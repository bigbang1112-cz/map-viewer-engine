using MapViewerEngine.Shared;
using Microsoft.AspNetCore.SignalR;

namespace MapViewerEngine.Server;

public interface IMapViewerEngineHub
{
    Task<byte[]> BlockMesh(BlockVariant block);
    Task<byte[]> Mesh(Guid guid);
    string Ping();
    void Dispose();
}

public class MapViewerEngineHub : Hub, IMapViewerEngineHub
{
    private readonly IMapViewerEngineRepo _repo;

    public MapViewerEngineHub(IMapViewerEngineRepo repo)
    {
        _repo = repo;
    }

    public string Ping()
    {
        return "Pong";
    }

    public async Task<byte[]> BlockMesh(BlockVariant block)
    {
        _ = block ?? throw new ArgumentNullException(nameof(block));

        var data = await _repo.GetBlockMeshDataAsync(block);

        if (data is null)
        {
            throw new Exception("Shouldn't happen or subject to more intense rate limiting");
        }

        return data;
    }

    public async Task<byte[]> Mesh(Guid guid)
    {
        var data = await _repo.GetMeshDataAsync(guid);

        if (data is null)
        {
            throw new Exception("Shouldn't happen or subject to more intense rate limiting");
        }

        return data;
    }
}
