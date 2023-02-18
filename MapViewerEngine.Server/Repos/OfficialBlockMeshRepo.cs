using GbxToolAPI.Server.Options;
using MapViewerEngine.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MapViewerEngine.Server.Repos;

public interface IOfficialBlockMeshRepo
{
    Task<byte[]?> GetDataAsync(BlockVariant block, CancellationToken cancellationToken = default);
}

public class OfficialBlockMeshRepo : IOfficialBlockMeshRepo
{
    private readonly MapViewerEngineContext context;
    private readonly IOptions<DatabaseOptions> options;

    public OfficialBlockMeshRepo(MapViewerEngineContext context, IOptions<DatabaseOptions> options)
    {
        this.context = context;
        this.options = options;
    }

    private bool IsInMemory => options.Value.InMemory;

    public async Task<byte[]?> GetDataAsync(BlockVariant block, CancellationToken cancellationToken = default)
    {
        _ = block ?? throw new ArgumentNullException(nameof(block));

        if (IsInMemory)
        {
            return await context.OfficialBlockMeshes
                .Include(bm => bm.OfficialBlock)
                .Where(bm => bm.OfficialBlock.Name == block.Name && bm.Ground == block.Ground && bm.Variant == block.Variant && bm.SubVariant == block.SubVariant)
                .Select(bm => bm.Mesh.Data)
                .FirstOrDefaultAsync(cancellationToken);
        }

        throw new NotImplementedException();
    }
}
