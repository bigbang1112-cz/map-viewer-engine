using Dapper;
using GbxToolAPI.Server;
using GbxToolAPI.Server.Options;
using MapViewerEngine.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.Common;

namespace MapViewerEngine.Server.Repos;

public interface IMeshRepo
{
    Task<byte[]?> GetDataAsync(Guid guid, CancellationToken cancellationToken = default);
}

public class MeshRepo : IMeshRepo
{
    private readonly MapViewerEngineContext context;
    private readonly ISqlConnection<MapViewerEngineServer> sql;
    private readonly IOptions<DatabaseOptions> options;

    public MeshRepo(MapViewerEngineContext context, ISqlConnection<MapViewerEngineServer> sql, IOptions<DatabaseOptions> options)
    {
        this.context = context;
        this.sql = sql;
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

        return await sql.Connection.QueryFirstOrDefaultAsync<byte[]?>("SELECT Data FROM Meshes WHERE Guid = @Guid", new { Guid = guid });
    }
}
