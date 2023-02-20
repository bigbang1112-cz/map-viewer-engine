using Dapper;
using GbxToolAPI.Server.Options;
using MapViewerEngine.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;

namespace MapViewerEngine.Server.Repos;

public interface IOfficialBlockMeshRepo
{
    Task<byte[]?> GetDataAsync(BlockVariant block, CancellationToken cancellationToken = default);
}

public class OfficialBlockMeshRepo : IOfficialBlockMeshRepo
{
    private readonly MapViewerEngineContext context;
    private readonly IDbConnection dbConnection;
    private readonly IOptions<DatabaseOptions> options;

    public OfficialBlockMeshRepo(MapViewerEngineContext context, IDbConnection dbConnection, IOptions<DatabaseOptions> options)
    {
        this.context = context;
        this.dbConnection = dbConnection;
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

        return await dbConnection.QueryFirstOrDefaultAsync<byte[]?>(
            "SELECT m.Data FROM officialblockmeshes bm " +
            "INNER JOIN meshes m ON bm.MeshId = m.id " +
            "WHERE bm.OfficialBlockId = (SELECT id FROM officialblocks WHERE name = @Name) " +
            "AND bm.Ground = @Ground AND bm.Variant = @Variant AND bm.SubVariant = @SubVariant",
            new { block.Name, block.Ground, block.Variant, block.SubVariant });
    }
}
