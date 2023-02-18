using GbxToolAPI.Server.Options;
using MapViewerEngine.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace MapViewerEngine.Server;

public interface IMapViewerEngineRepo
{
    Task<byte[]?> GetBlockMeshDataAsync(BlockVariant block, CancellationToken cancellationToken = default);
    Task<byte[]?> GetMeshDataAsync(Guid guid, CancellationToken cancellationToken = default);
}

public class MapViewerEngineRepo : IMapViewerEngineRepo
{
    private readonly MapViewerEngineContext context;
    private readonly IOptions<DatabaseOptions> options;

    public MapViewerEngineRepo(MapViewerEngineContext context, IOptions<DatabaseOptions> options)
    {
        this.context = context;
        this.options = options;
    }

    private bool IsInMemory => options.Value.InMemory;

    public async Task<byte[]?> GetBlockMeshDataAsync(BlockVariant block, CancellationToken cancellationToken = default)
    {
        _ = block ?? throw new ArgumentNullException(nameof(block));

        if (IsInMemory)
        {
            return await context.OfficialBlockMeshes
                .Where(bm => bm.OfficialBlock.Name == block.Name && bm.Ground == block.Ground && bm.Variant == block.Variant && bm.SubVariant == block.SubVariant)
                .Select(bm => bm.Mesh.Data)
                .FirstOrDefaultAsync(cancellationToken);
        }

        throw new NotImplementedException();
    }

    public async Task<byte[]?> GetMeshDataAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        if (IsInMemory)
        {
            return await context.Meshes
                .Where(m => m.Guid == guid)
                .Select(bm => bm.Data)
                .FirstOrDefaultAsync(cancellationToken);
        }
        
        throw new NotImplementedException();
    }
}
