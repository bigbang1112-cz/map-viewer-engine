using MapViewerEngine.Shared;
using Microsoft.AspNetCore.SignalR;

namespace MapViewerEngine.Server;

public class MapViewerHub : Hub
{
    private readonly IMapViewerRepo _repo;

    public MapViewerHub(IMapViewerRepo repo)
    {
        _repo = repo;
    }

    public async Task BlockMesh(BlockVariant block, CancellationToken cancellationToken = default)
    {
        var mesh = await _repo.GetBlockMeshAsync(block, cancellationToken);

        await Clients.Caller.SendAsync(nameof(BlockMesh), block, mesh, cancellationToken);
    }

    public async Task Mesh(Guid guid, CancellationToken cancellationToken = default)
    {
        var mesh = await _repo.GetBlockMeshAsync(guid, cancellationToken);

        await Clients.Caller.SendAsync(nameof(Mesh), guid, mesh, cancellationToken);
    }
}
