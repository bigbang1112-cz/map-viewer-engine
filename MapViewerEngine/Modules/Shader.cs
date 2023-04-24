using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;
using System.Xml.Linq;

namespace MapViewerEngine.Modules;

internal static partial class Shader
{
    private static readonly byte[] MAGIC = new byte[] { 0x35, 0x84, 0x14, 0x44, 0x54 };

    private const int VERSION = 0;

    [JSImport("loadTexture", nameof(Shader))]
    public static partial JSObject LoadTexture(string path);

    [JSImport("set", nameof(Shader))]
    public static partial void Set(JSObject mesh, JSObject shader);

    [JSImport("setTexture", nameof(Shader))]
    public static partial bool SetTexture(JSObject shader, JSObject texture, string name);

    [JSImport("create", nameof(Shader))]
    public static partial JSObject? Create(string name);

    [JSImport("animate", nameof(Shader))]
    public static partial void Animate();

    public static void Update(JSObject existingMaterial, byte[] data)
    {
        using var stream = new MemoryStream(data);
        using var deflate = new DeflateStream(stream, CompressionMode.Decompress);
        using var reader = new BinaryReader(deflate);

        var magic = reader.ReadBytes(5);

        if (!magic.SequenceEqual(MAGIC))
        {
            throw new InvalidDataException("Invalid file format");
        }

        var version = reader.Read7BitEncodedInt();

        var textureDict = new Dictionary<string, string>();

        var textureCount = reader.Read7BitEncodedInt();
        for (int i = 0; i < textureCount; i++)
        {
            var textureName = reader.ReadString();
            var textureGbx = reader.ReadString();
            var textureFileName = reader.ReadString();

            textureDict.Add(textureName, textureFileName);
        }

        if (textureDict.TryGetValue("Diffuse", out var texturePath))
        {
            SetTexture(existingMaterial, "Diffuse", texturePath);
            return;
        }

        foreach (var (textureName, textureFileName) in textureDict)
        {
            if (SetTexture(existingMaterial, textureName, textureFileName))
            {
                return;
            }
        }
    }

    private static bool SetTexture(JSObject existingMaterial, string textureName, string textureFileName)
    {
        if (!MapViewerEngineTool.CachedTextures.TryGetValue(textureFileName, out var texture))
        {
            texture = LoadTexture("textures/" + textureFileName);
            MapViewerEngineTool.CachedTextures.Add(textureFileName, texture);
        }

        return SetTexture(existingMaterial, texture, textureName);
    }

    private static readonly HashSet<string> waterMaterials = new()
    {
        "SpeedWater",
        "BaySea",
        "BayWarpSea",
        "CoastSea",
        "CoastWarpSea",
        "CoastFoam",
        "RallyWater",
        "RallyWarpLake",
        "StadiumWater"
    };

    internal static bool IsWater(string shaderName)
    {
        return waterMaterials.Contains(shaderName);
    }
}
