using GbxToolAPI.Client;
using MapViewerEngine.Shared;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace MapViewerEngine;

public delegate Task BlockMeshHandler(BlockVariant block, byte[] data);
public delegate Task MeshHandler(Guid guid, byte[] data);
public delegate Task MetaHandler(string blockName, string collection, byte[] data);
public delegate Task MetasHandler(OfficialBlockMeta[] metas);

public partial class MapViewerEngineHubConnection : ToolHubConnection
{
    public event BlockMeshHandler? BlockMesh;
    public event MeshHandler? Mesh;
    public event MetaHandler? Meta;
    public event MetasHandler? Metas;

    public MapViewerEngineHubConnection(string baseAddress, ILogger? logger = null) : base(baseAddress, logger)
    {
        Connection.On<BlockVariant, byte[]>("BlockMesh", OnBlockMesh);
        Connection.On<Guid, byte[]>("Mesh", OnMesh);
        Connection.On<string, string, byte[]>("Meta", OnMeta);
        Connection.On<OfficialBlockMeta[]>("Metas", OnMetas);
    }

    public async Task<string> PingAsync(CancellationToken cancellationToken = default)
    {
        return await Connection.InvokeAsync<string>("Ping", cancellationToken);
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

    protected virtual async Task OnMeta(string blockName, string collection, byte[] data)
    {
        if (Meta is not null)
        {
            await Meta.Invoke(blockName, collection, data);
        }
    }

    protected virtual async Task OnMetas(OfficialBlockMeta[] metas)
    {
        if (Metas is not null)
        {
            await Metas.Invoke(metas);
        }
    }

    public async Task SendBlockMeshAsync(BlockVariant block, CancellationToken cancellationToken = default)
    {
        await Connection.SendAsync("BlockMesh", block, cancellationToken);
    }

    public async Task SendMeshAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        await Connection.SendAsync("Mesh", guid, cancellationToken);
    }

    public async Task SendMetaAsync(string blockName, string collection, string author, CancellationToken cancellationToken = default)
    {
        await Connection.SendAsync("Meta", blockName, collection, author, cancellationToken);
    }

    public async Task SendMetasAsync(IEnumerable<string> blockNames, string collection, string author, CancellationToken cancellationToken = default)
    {
        await Connection.SendAsync("Metas", blockNames, collection, author, cancellationToken);
    }
}
