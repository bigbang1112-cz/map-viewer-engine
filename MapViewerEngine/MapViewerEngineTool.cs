﻿using GBX.NET;
using GBX.NET.Engines.Game;
using GbxToolAPI;
using GbxToolAPI.Client;
using MapViewerEngine.Modules;
using MapViewerEngine.Shared;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;

namespace MapViewerEngine;

[ToolName("Map Viewer Engine")]
[ToolDescription("View maps in a web browser.")]
[SupportedOSPlatform("browser")]
public class MapViewerEngineTool : ITool, IHasUI, IHubConnection<MapViewerEngineHubConnection>
{
    public CGameCtnChallenge Map { get; }
    public Int3 MapSize { get; }
    public Int3 EnvBlockSize { get; }
    public Int3 TrueMapSize { get; }
    public Int3 TrueMapCenter { get; }
    public Vec3 AbsoluteTrueMapCenter { get; }
    public BlockVariant? ZoneVariant { get; }
    public int[] ZoneBlockPlacements { get; } = Array.Empty<int>();

    public Dictionary<BlockVariant, int> BlockCountPerModel { get; }

    public required MapViewerEngineHubConnection HubConnection { get; init; }

    public static readonly Dictionary<BlockVariant, JSObject> CachedSolids = new();
    public static readonly Dictionary<string, Meta> CachedMetas = new();
    public static readonly Dictionary<string, List<(BlockVariant, JSObject)>> SolidsToInstantiateLater = new();
    public static readonly Dictionary<string, JSObject> CachedShaders = new();
    public static readonly Dictionary<string, bool> RequestedShaders = new();
    public static readonly Dictionary<string, JSObject> CachedTextures = new();
    public static readonly Dictionary<(string, int, int), JSObject> CachedScenes = new();

    public MapViewerEngineTool(CGameCtnChallenge map)
	{
        Map = map;
        MapSize = map.Size ?? throw new NullReferenceException("Map size is null");
        EnvBlockSize = GetBlockSize(map);
        TrueMapSize = GetTrueMapSize(map, out var lowerCoord, out var higherCoord);
        TrueMapCenter = lowerCoord + ((higherCoord.X - lowerCoord.X) / 2, higherCoord.Y, (higherCoord.Z - lowerCoord.Z) / 2);
        AbsoluteTrueMapCenter = TrueMapCenter * EnvBlockSize + (EnvBlockSize * 0.5f);
        
        BlockCountPerModel = map.GetBlocks()
            .GroupBy(x => new { x.Name, x.IsGround, x.Variant, x.SubVariant })
            .ToDictionary(x => new BlockVariant(x.Key.Name, x.Key.IsGround, x.Key.Variant.GetValueOrDefault(), x.Key.SubVariant.GetValueOrDefault()), x => x.Count());

        ZoneVariant = GetEnvironmentZoneVariant();

        if (ZoneVariant is not null)
        {
            ZoneBlockPlacements = GetBlockPlacementsOfZone();

            if (BlockCountPerModel.ContainsKey(ZoneVariant))
            {
                BlockCountPerModel[ZoneVariant] += ZoneBlockPlacements.Length;
            }
            else
            {
                BlockCountPerModel[ZoneVariant] = ZoneBlockPlacements.Length;
            }
        }
    }

    private static Int3 GetBlockSize(CGameCtnChallenge map) => map.Collection.ToString() switch
    {
        "Desert" or "Speed" or "Snow" or "Alpine" => (32, 16, 32),
        "Island" => (64, 8, 64),
        "Rally" or "Bay" or "Stadium" or "Valley" or "Lagoon" or "Stadium2020" => (32, 8, 32),
        "Coast" => (16, 4, 16),
        "Canyon" => (64, 16, 64),
        _ => throw new NotSupportedException("Block size not supported for this collection"),
    };

    private static Int3 GetTrueMapSize(CGameCtnChallenge map, out Int3 lowerCoord, out Int3 higherCoord)
    {
        higherCoord = new Int3();
        lowerCoord = map.Size.GetValueOrDefault();

        foreach (var block in map.GetBlocks())
        {
            if (block.Coord.X > higherCoord.X) higherCoord = higherCoord with { X = block.Coord.X };
            if (block.Coord.Y > higherCoord.Y) higherCoord = higherCoord with { Y = block.Coord.Y };
            if (block.Coord.Z > higherCoord.Z) higherCoord = higherCoord with { Z = block.Coord.Z };

            if (block.Coord.X < lowerCoord.X) lowerCoord = lowerCoord with { X = block.Coord.X };
            if (block.Coord.Y < lowerCoord.Y) lowerCoord = lowerCoord with { Y = block.Coord.Y };
            if (block.Coord.Z < lowerCoord.Z) lowerCoord = lowerCoord with { Z = block.Coord.Z };
        }

        return map.NbBlocks == 0 ? map.Size.GetValueOrDefault() : higherCoord - lowerCoord + (1, 1, 1);
    }

    public async Task LoadAsync(CancellationToken cancellationToken = default)
    {
        var uniqueBlockNames = Map.GetBlocks()
            .Select(x => x.Name)
            .Distinct()
            .ToList();

        var blockInfosToRequest = uniqueBlockNames.Where(x => !CachedMetas.ContainsKey(x)).ToList();

        await HubConnection.SendMetasAsync(blockInfosToRequest, Map.Collection, "Nadeo", cancellationToken);

        var uniqueBlockVariants = Map.GetBlocks()
            .GroupBy(x => new { x.Name, x.IsGround, x.Variant, x.SubVariant })
            .Select(x => new BlockVariant(x.Key.Name, x.Key.IsGround, x.Key.Variant.GetValueOrDefault(), x.Key.SubVariant.GetValueOrDefault()))
            .ToList();

        if (ZoneVariant is not null)
        {
            uniqueBlockVariants.Remove(ZoneVariant);
            uniqueBlockVariants.Insert(0, ZoneVariant); // so that it shows first
        }

        await CreateSceneAsync(cancellationToken);

        foreach (var variant in uniqueBlockVariants)
        {
            if (CachedSolids.TryGetValue(variant, out var obj))
            {
                Solid.UpdateInstanceCount(obj, BlockCountPerModel[variant]);

                Renderer.AddToScene(obj);

                if (variant == ZoneVariant)
                {
                    InstantiateDefaultZoneSolids(obj);
                    continue;
                }

                if (CachedMetas.TryGetValue(variant.Name, out var meta))
                {
                    InstantiateSolids(obj, variant, meta);
                }
                else
                {
                    if (SolidsToInstantiateLater.TryGetValue(variant.Name, out var list))
                    {
                        list.Add((variant, obj));
                    }
                    else
                    {
                        SolidsToInstantiateLater.Add(variant.Name, new List<(BlockVariant, JSObject)> { (variant, obj) });
                    }
                }

                continue;
            }

            try // have a client side list of accepted block names instead (to respect serverside)
            {
                await HubConnection.SendBlockMeshAsync(variant, cancellationToken);
            }
            catch
            {

            }

            await Task.Delay(20, cancellationToken);
        }
    }

    private BlockVariant? GetEnvironmentZoneVariant()
    {
        var blockName = Map.Collection.ToString() switch
        {
            "Desert" or "Speed" => "Canyon",
            "Snow" or "Alpine" => "AlpineSnowBase",
            "Rally" => "RallyFlat",
            "Island" => "IslandSea",
            "Bay" => "BaySea",
            "Coast" => "CoastSea",
            "Stadium" => "StadiumGrass",
            _ => null,
        };

        if (blockName is null)
        {
            return null;
        }

        return new BlockVariant(blockName, true, 0, 0);
    }

    private static bool IsDefaultZone(string blockName) => blockName switch
    {
        "Canyon" => true,
        "Snow" => true,
        "RallyFlat" => true,
        "IslandSea" => true,
        "BaySea" => true,
        "CoastSea" => true,
        "StadiumGrass" => true,
        _ => false,
    };

    private async Task CreateSceneAsync(CancellationToken cancellationToken)
    {
        if (!CachedScenes.TryGetValue((Map.Collection, MapSize.X, MapSize.Z), out var sceneObj))
        {
            byte[] sceneMesh;
            
            try
            {
                sceneMesh = await HubConnection.InvokeSceneAsync(Map.Collection, MapSize.X, MapSize.Z, cancellationToken);
            }
            catch
            {
                return;
            }

            sceneObj = await Solid.ParseAsync(sceneMesh, 1);
            CachedScenes.Add((Map.Collection, MapSize.X, MapSize.Z), sceneObj);

            var sceneYOffset = Map.Collection.ToString() switch
            {
                "Stadium" => EnvBlockSize.Y + 1,
                "Bay" => 2 * EnvBlockSize.Y,
                "Coast" => 3 * EnvBlockSize.Y,
                _ => 0
            };

            Solid.SetTreePos(sceneObj, 0, sceneYOffset, 0);
        }
        
        
        Renderer.AddToScene(sceneObj);
    }

    private static readonly Dictionary<string, int> frontierScrollBlocks = new()
    {
        { "BayEsplanade", 4 },
        { "CoastDirtCliff", 3 },
        { "CoastWaterCliff", 3 },
        { "IslandHills6", 6 },
        { "StadiumDirtHill", 4 },
        { "SnowToSnow", 3 },
        { "DesertToRock", 2 },
        { "RockToRock", 4 },
        { "DesertToDesert", 3 },
        { "Hills", 1 },
        { "HillsToHills", 2 }
    };

    public void InstantiateDefaultZoneSolids(JSObject tree)
    {
        Solid.Instantiate(tree, ZoneBlockPlacements, 1, 1, EnvBlockSize.X, EnvBlockSize.Y, EnvBlockSize.Z);
    }

    public void InstantiateSolids(JSObject tree, BlockVariant variant, Meta meta)
    {        
        var blockSize = variant.Ground ? meta.GroundBlockSize : meta.AirBlockSize;

        _ = frontierScrollBlocks.TryGetValue(variant.Name, out var offsetY);
        
        var blockPlacements = Map.GetBlocks()
            .Where(x => x.Name == variant.Name && x.IsGround == variant.Ground && x.Variant == variant.Variant && x.SubVariant == variant.SubVariant)
            .Select(x => (x.Coord.X << 24) | ((x.Coord.Y - offsetY) << 16) | (x.Coord.Z << 8) | ((int)x.Direction & 0x0F))
            .ToArray();

        Solid.Instantiate(tree, blockPlacements, blockSize.X, blockSize.Z, EnvBlockSize.X, EnvBlockSize.Y, EnvBlockSize.Z);
    }

    private int[] GetBlockPlacementsOfZone()
    {
        var dontPlaceAt = new bool[MapSize.X, MapSize.Z];

        var groundY = Map.Collection.ToString() switch
        {
            "Stadium" => 1,
            "Bay" or "Alpine" or "Speed" or "Desert" => 4,
            "Rally" or "Island" => 5,
            "Coast" => 6,
            _ => 0
        };

        var additionalY = Map.Collection.ToString() switch
        {
            "Speed" or "Desert" => 1,
            "Stadium" or "Alpine" => -1,
            "Rally" => -2,
            "Coast" => -3,
            _ => 0
        };

        foreach (var block in Map.GetBlocks())
        {
            if (block.Coord.Y <= groundY)
            {
                dontPlaceAt[block.Coord.X, block.Coord.Z] = true;
            }
        }

        var blockPlacements = new List<int>();

        for (int x = 0; x < MapSize.X; x++)
        {
            for (int z = 0; z < MapSize.Z; z++)
            {
                if (!dontPlaceAt[x, z])
                {
                    blockPlacements.Add((x << 24) | ((groundY + additionalY) << 16) | (z << 8));
                }
            }
        }

        return blockPlacements.ToArray();
    }
}
