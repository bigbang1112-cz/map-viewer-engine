using GbxToolAPI.Server.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MapViewerEngine.Server.Repos;

public interface IMeshRepo
{
    Task<byte[]?> GetDataAsync(Guid guid, CancellationToken cancellationToken = default);
}

public class MeshRepo : IMeshRepo
{
    private readonly MapViewerEngineContext context;
    private readonly IOptions<DatabaseOptions> options;

    public MeshRepo(MapViewerEngineContext context, IOptions<DatabaseOptions> options)
    {
        this.context = context;
        this.options = options;
    }

    private bool IsInMemory => options.Value.InMemory;

    public async Task<byte[]?> GetDataAsync(Guid guid, CancellationToken cancellationToken = default)
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
