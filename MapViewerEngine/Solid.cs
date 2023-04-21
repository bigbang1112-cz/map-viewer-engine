using GBX.NET;
using System;
using System.IO.Compression;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.JavaScript;

namespace MapViewerEngine;

internal static partial class Solid
{
    private static readonly byte[] MAGIC = new byte[] { 0xD4, 0x54, 0x35, 0x84, 0x02 };
    
    private const int VERSION = 0;

    [JSImport("create_tree", nameof(Solid))]
    internal static partial JSObject CreateTree();

    [JSImport("wrap_tree", nameof(Solid))]
    internal static partial JSObject WrapTree(JSObject tree);

    [JSImport("hide_tree", nameof(Solid))]
    internal static partial void HideTree(JSObject tree);

    [JSImport("add_to_tree", nameof(Solid))]
    internal static partial void AddToTree(JSObject parent, JSObject child);

    [JSImport("set_tree_pos", nameof(Solid))]
    internal static partial void SetTreePos(JSObject tree, double x, double y, double z);

    [JSImport("set_tree_rot", nameof(Solid))]
    internal static partial void SetTreeRot(JSObject tree, double xx, double xy, double xz, double yx, double yy, double yz, double zx, double zy, double zz);

    [JSImport("create_visual", nameof(Solid))]
    internal static partial JSObject CreateVisual([JSMarshalAs<JSType.MemoryView>] Span<byte> vertices, [JSMarshalAs<JSType.MemoryView>] Span<int> indices, [JSMarshalAs<JSType.MemoryView>] Span<byte> uvs, int expectedBlockCount);

    [JSImport("create_lod", nameof(Solid))]
    internal static partial JSObject CreateLod();

    [JSImport("add_lod", nameof(Solid))]
    internal static partial void AddLod(JSObject lodTree, JSObject levelTree, double distance);

    [JSImport("instantiate", nameof(Solid))]
    internal static partial void Instantiate(JSObject tree, int[] placements, int blockSizeX, int blockSizeZ, int envBlockSizeX, int envBlockSizeY, int envBlockSizeZ);

    [JSImport("setUserData", nameof(Solid))]
    internal static partial void SetUserData(JSObject tree, string treeName, string? shaderName);

    [JSImport("addUserData", nameof(Solid))]
    internal static partial void AddUserData(JSObject tree, string name, bool isGround, int variant, int subVariant);

    [JSImport("setShader", nameof(Solid))]
    internal static partial void SetShader(JSObject tree, JSObject shader);

    public static async Task<JSObject> ParseAsync(byte[] data, int expectedBlockCount)
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
        
        var hasFileWriteTime = reader.ReadBoolean();
        var fileWriteTime = hasFileWriteTime ? reader.ReadInt64() : 0;

        return WrapTree(await ReadTreeAsync(reader, expectedBlockCount));
    }

    private static async Task<JSObject> ReadTreeAsync(BinaryReader reader, int expectedBlockCount)
    {
        var tree = CreateTree();

        var childrenCount = reader.Read7BitEncodedInt();
        for (int i = 0; i < childrenCount; i++)
        {
            AddToTree(tree, await ReadTreeAsync(reader, expectedBlockCount));
        }

        if (reader.ReadBoolean())
        {
            var mat3 = ReadMatrix3(reader);
            SetTreeRot(tree, mat3.XX, mat3.XY, mat3.XZ, mat3.YX, mat3.YY, mat3.YZ, mat3.ZX, mat3.ZY, mat3.ZZ);
        }

        if (reader.ReadBoolean())
        {
            var pos = ReadVector3(reader);
            SetTreePos(tree, pos.X, pos.Y, pos.Z);
        }        
        
        var visual = ReadVisual(reader, expectedBlockCount);

        if (visual is not null)
        {
            AddToTree(tree, visual);
        }

        var mipLevelCount = reader.Read7BitEncodedInt();

        if (mipLevelCount > 0)
        {
            var lod = CreateLod();

            var storedDistance = 0f;

            for (int i = 0; i < mipLevelCount; i++)
            {
                var distance = storedDistance;
                storedDistance = reader.ReadSingle() * 2;

                AddLod(lod, await ReadTreeAsync(reader, expectedBlockCount), distance);

                await Task.Delay(10);
            }

            AddToTree(tree, lod);
        }

        var hasShader = reader.ReadBoolean();
        var shaderName = hasShader ? reader.ReadString() : null;

        var name = reader.ReadString();

        SetUserData(tree, name, shaderName);

        if (shaderName is not null)
        {
            if (MapViewerEngineTool.CachedShaders.TryGetValue(shaderName, out var shader))
            {
                if (shader is not null)
                {
                    SetShader(tree, shader);
                }
            }
            else
            {
                MapViewerEngineTool.RequestedShaders.TryAdd(shaderName, false);
            }
        }

        return tree;
    }

    private static JSObject? ReadVisual(BinaryReader reader, int expectedBlockCount)
    {
        var hasVisual = reader.ReadBoolean();
        
        if (!hasVisual)
        {
            return null;
        }
        
        var vertexCount = reader.Read7BitEncodedInt();
        var texSetCount = reader.Read7BitEncodedInt();

        // Parse texture coordinates        
        Span<byte> uvs = reader.ReadBytes(texSetCount * vertexCount * 2 * sizeof(float));

        // Parse vertices
        Span<byte> vertices = reader.ReadBytes(vertexCount * 3 * sizeof(float));

        // Parse indices
        Span<int> indices = stackalloc int[reader.Read7BitEncodedInt()];
        for (int i = 0; i < indices.Length; i++)
        {
            indices[i] = reader.Read7BitEncodedInt();
        }

        return CreateVisual(vertices, indices, uvs, expectedBlockCount);
    }

    private static Mat3 ReadMatrix3(BinaryReader reader)
    {
        var xx = reader.ReadSingle();
        var xy = reader.ReadSingle();
        var xz = reader.ReadSingle();
        var yx = reader.ReadSingle();
        var yy = reader.ReadSingle();
        var yz = reader.ReadSingle();
        var zx = reader.ReadSingle();
        var zy = reader.ReadSingle();
        var zz = reader.ReadSingle();

        return new Mat3(xx, xy, xz, yx, yy, yz, zx, zy, zz);
    }

    private static Vector3 ReadVector3(BinaryReader reader)
    {
        var x = reader.ReadSingle();
        var y = reader.ReadSingle();
        var z = reader.ReadSingle();

        return new Vector3(x, y, z);
    }
}
