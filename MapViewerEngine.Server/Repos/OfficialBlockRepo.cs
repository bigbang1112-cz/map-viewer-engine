using Dapper;
using GbxToolAPI.Server.Options;
using MapViewerEngine.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;

namespace MapViewerEngine.Server.Repos;

public interface IOfficialBlockRepo
{
    Task<IEnumerable<OfficialBlock>> GetAllAsync(CancellationToken cancellationToken = default);
}

public class OfficialBlockRepo : IOfficialBlockRepo
{
    private readonly MapViewerEngineContext context;
    private readonly IDbConnection dbConnection;
    private readonly IOptions<DatabaseOptions> options;

    public OfficialBlockRepo(MapViewerEngineContext context, IDbConnection dbConnection, IOptions<DatabaseOptions> options)
    {
        this.context = context;
        this.dbConnection = dbConnection;
        this.options = options;
    }

    private bool IsInMemory => options.Value.InMemory;

    public async Task<IEnumerable<OfficialBlock>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        if (IsInMemory)
        {
            return await context.OfficialBlocks.ToListAsync(cancellationToken);
        }

        return await dbConnection.QueryAsync<OfficialBlock>("SELECT * FROM OfficialBlocks");
    }
}
