using GbxToolAPI.Server.Options;
using GbxToolAPI.Server;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Dapper;

namespace MapViewerEngine.Server.Repos;

public interface IOfficialShaderRepo
{
    Task<byte[]?> GetByNameAsync(string shaderName, CancellationToken cancellationToken = default);
}

public class OfficialShaderRepo : IOfficialShaderRepo
{
    private readonly MapViewerEngineContext context;
    private readonly ISqlConnection<MapViewerEngineServer> sql;
    private readonly IOptions<DatabaseOptions> options;

    public OfficialShaderRepo(MapViewerEngineContext context, ISqlConnection<MapViewerEngineServer> sql, IOptions<DatabaseOptions> options)
    {
        this.context = context;
        this.sql = sql;
        this.options = options;
    }

    private bool IsInMemory => options.Value.InMemory;

    public async Task<byte[]?> GetByNameAsync(string shaderName, CancellationToken cancellationToken = default)
    {
        if (IsInMemory)
        {
            return await context.OfficialShaders
                .Where(s => s.Name == shaderName && string.IsNullOrEmpty(s.Modifier))
                .Select(s => s.Data)
                .FirstOrDefaultAsync(cancellationToken);
        }

        return await sql.Connection.QueryFirstOrDefaultAsync<byte[]?>(
            @"SELECT Data FROM OfficialShaders WHERE Name = @Name AND (Modifier = '' OR LENGTH(Modifier) = 0)",
            new { Name = shaderName });
    }
}
