using MapViewerEngine.Server.Repos;
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
    private readonly IMeshRepo meshRepo;
    private readonly IOfficialBlockMeshRepo officialBlockMeshRepo;

    public MapViewerEngineHub(IMeshRepo meshRepo, IOfficialBlockMeshRepo officialBlockMeshRepo)
    {
        this.meshRepo = meshRepo;
        this.officialBlockMeshRepo = officialBlockMeshRepo;
    }

    public string Ping()
    {
        return "Pong";
    }

    public async Task<byte[]> BlockMesh(BlockVariant block)
    {
        _ = block ?? throw new ArgumentNullException(nameof(block));

        var data = await officialBlockMeshRepo.GetDataAsync(block);

        if (data is null)
        {
            throw new Exception("Shouldn't happen or subject to more intense rate limiting");
        }

        return data;
    }

    public async Task<byte[]> Mesh(Guid guid)
    {
        var data = await meshRepo.GetDataAsync(guid);

        if (data is null)
        {
            throw new Exception("Shouldn't happen or subject to more intense rate limiting");
        }

        return data;
    }
}
