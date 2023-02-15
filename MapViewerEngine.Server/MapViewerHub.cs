using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace MapViewerEngine.Server;

public class MapViewerHub : Hub
{
    private readonly IMapViewerRepo repo;

    public MapViewerHub(IMapViewerRepo repo)
    {
        this.repo = repo;
    }

    public async Task SendBlockMesh(string blockName, bool ground, int variant, int subVariant, CancellationToken cancellationToken = default)
    {
        var mesh = await repo.GetBlockMeshAsync(blockName, ground, variant, subVariant, cancellationToken);

        await Clients.Caller.SendAsync("ReceiveBlockMesh", mesh, cancellationToken);
    }

    public async Task SendMesh(Guid guid, CancellationToken cancellationToken = default)
    {
        var mesh = await repo.GetBlockMeshAsync(guid, cancellationToken);

        await Clients.Caller.SendAsync("ReceiveMesh", mesh, cancellationToken);
    }
}
