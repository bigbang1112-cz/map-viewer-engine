using GBX.NET.Engines.Game;
using GbxToolAPI;
using GbxToolAPI.Client;
using MapViewerEngine.Shared;

namespace MapViewerEngine;

[ToolName("Map Viewer Engine")]
[ToolDescription("View maps in a web browser.")]
public class MapViewerEngineTool : ITool, IHasUI, IHubConnection<MapViewerEngineHubConnection>
{
    private readonly CGameCtnChallenge map;

    public required MapViewerEngineHubConnection HubConnection { get; init; }

    public MapViewerEngineTool(CGameCtnChallenge map)
	{
        this.map = map;
    }

    public async Task LoadAsync(CancellationToken cancellationToken = default)
    {
        var uniqueBlockNames = map.GetBlocks()
            .Select(x => x.Name)
            .Distinct()
            .ToList();

        await HubConnection.SendMetasAsync(uniqueBlockNames, map.Collection, "Nadeo", cancellationToken);

        var uniqueBlockVariants = map.GetBlocks()
            .GroupBy(x => new { x.Name, x.IsGround, x.Variant, x.SubVariant })
            .Select(x => new BlockVariant(x.Key.Name, x.Key.IsGround, x.Key.Variant.GetValueOrDefault(), x.Key.SubVariant.GetValueOrDefault()))
            .ToList();

        foreach (var variant in uniqueBlockVariants)
        {
            try // have a client side list of accepted block names instead (to respect serverside)
            {
                await HubConnection.SendBlockMeshAsync(variant, cancellationToken);
            }
            catch
            {

            }
        }
    }
}
