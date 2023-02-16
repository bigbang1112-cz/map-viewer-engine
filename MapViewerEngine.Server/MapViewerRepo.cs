using MapViewerEngine.Shared;
using Microsoft.Extensions.Configuration;

namespace MapViewerEngine.Server;

public interface IMapViewerRepo
{
    Task<byte[]> GetBlockMeshAsync(BlockVariant block, CancellationToken cancellationToken = default);
    Task<byte[]> GetBlockMeshAsync(Guid guid, CancellationToken cancellationToken = default);
}

public class MapViewerRepo : IMapViewerRepo
{
    private readonly IConfiguration config;

    public MapViewerRepo(IConfiguration config)
    {
        this.config = config;
    }

    public Task<byte[]> GetBlockMeshAsync(BlockVariant block, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<byte[]> GetBlockMeshAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
