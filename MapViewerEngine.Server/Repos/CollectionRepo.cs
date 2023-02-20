using Dapper;
using GbxToolAPI.Server.Options;
using MapViewerEngine.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;

namespace MapViewerEngine.Server.Repos;

public interface ICollectionRepo
{
    Task AddAsync(Collection collection, CancellationToken cancellationToken = default);
    Task<IEnumerable<Collection>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Collection?> GetByIdAsync(int id, CancellationToken cancellationToken);
}

public class CollectionRepo : ICollectionRepo
{
    private readonly MapViewerEngineContext context;
    private readonly IDbConnection dbConnection;
    private readonly IOptions<DatabaseOptions> options;

    public CollectionRepo(MapViewerEngineContext context, IDbConnection dbConnection, IOptions<DatabaseOptions> options)
    {
        this.context = context;
        this.dbConnection = dbConnection;
        this.options = options;
    }

    private bool IsInMemory => options.Value.InMemory;

    public async Task<IEnumerable<Collection>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        if (IsInMemory)
        {
            return await context.Collections.ToListAsync(cancellationToken);
        }

        return await dbConnection.QueryAsync<Collection>("SELECT * FROM Collections");
    }

    public async Task AddAsync(Collection collection, CancellationToken cancellationToken = default)
    {
        await context.Collections.AddAsync(collection, cancellationToken);
    }

    public async Task<Collection?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        if (IsInMemory)
        {
            return await context.Collections.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        return await dbConnection.QueryFirstOrDefaultAsync<Collection>("SELECT * FROM Collections WHERE Id = @Id", new { Id = id });
    }
}
