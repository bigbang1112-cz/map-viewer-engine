using GbxToolAPI.Client;
using MapViewerEngine.Shared;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace MapViewerEngine;

public delegate Task BlockMeshHandler(BlockVariant block, byte[] data);
public delegate Task MeshHandler(Guid guid, byte[] data);

public partial class MapViewerEngineHub : ToolHub
{
    public event BlockMeshHandler? BlockMesh;
    public event MeshHandler? Mesh;

    public MapViewerEngineHub(string baseAddress, ILogger<ToolHub>? logger = null) : base(baseAddress, logger)
    {
        Connection.On<BlockVariant, byte[]>("BlockMesh", OnBlockMesh);
        Connection.On<Guid, byte[]>("Mesh", OnMesh);
    }

    protected virtual async Task OnBlockMesh(BlockVariant block, byte[] data)
    {
        if (BlockMesh is not null)
        {
            await BlockMesh.Invoke(block, data);
        }
    }

    protected virtual async Task OnMesh(Guid guid, byte[] data)
    {
        if (Mesh is not null)
        {
            await Mesh.Invoke(guid, data);
        }
    }

    public async Task SendBlockMesh(BlockVariant block, CancellationToken cancellationToken = default)
    {
        await Connection.SendAsync("BlockMesh", block, cancellationToken);
    }

    public async Task SendMesh(Guid guid, CancellationToken cancellationToken = default)
    {
        await Connection.SendAsync("Mesh", guid, cancellationToken);
    }

    public async Task<byte[]> InvokeBlockMesh(string blockName,
                                              bool ground,
                                              int variant,
                                              int subVariant,
                                              CancellationToken cancellationToken = default)
    {
        return await Connection.InvokeAsync<byte[]>("BlockMesh", blockName, ground, variant, subVariant, cancellationToken);
    }

    public async Task<byte[]> InvokeMesh(Guid guid, CancellationToken cancellationToken = default)
    {
        return await Connection.InvokeAsync<byte[]>("Mesh", guid, cancellationToken);
    }
}
