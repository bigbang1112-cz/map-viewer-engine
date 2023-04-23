using GbxToolAPI.Server.Options;
using GbxToolAPI.Server;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Dapper;

namespace MapViewerEngine.Server.Repos;

public interface IOfficialSceneRepo
{
    Task<byte[]?> GetMeshDataByMapSizeAsync(string collection, int sizeX, int sizeZ, CancellationToken cancellationToken = default);
}

public class OfficialSceneRepo : IOfficialSceneRepo
{
    private readonly MapViewerEngineContext context;
    private readonly ISqlConnection<MapViewerEngineServer> sql;
    private readonly IOptions<DatabaseOptions> options;

    public OfficialSceneRepo(MapViewerEngineContext context, ISqlConnection<MapViewerEngineServer> sql, IOptions<DatabaseOptions> options)
    {
        this.context = context;
        this.sql = sql;
        this.options = options;
    }

    private bool IsInMemory => options.Value.InMemory;

    public async Task<byte[]?> GetMeshDataByMapSizeAsync(string collection, int sizeX, int sizeZ, CancellationToken cancellationToken = default)
    {
        if (IsInMemory)
        {
            return await context.OfficialScenes
                .Include(s => s.Mesh)
                .Include(s => s.Collection)
                .Where(s => s.SizeX == sizeX && s.SizeZ == sizeZ && s.Collection.Name == collection)
                .Select(s => s.Mesh.Data)
                .FirstOrDefaultAsync(cancellationToken);
        }
        
        return await sql.Connection.QueryFirstOrDefaultAsync<byte[]?>(
            @"SELECT Meshes.Data FROM OfficialScenes
                JOIN Meshes ON OfficialScenes.MeshId = Meshes.Id
                JOIN Collections c ON c.Id = OfficialScenes.CollectionId
                WHERE OfficialScenes.SizeX = @SizeX AND OfficialScenes.SizeZ = @SizeZ AND c.Name = @Collection",
            new { SizeX = sizeX, SizeZ = sizeZ, Collection = collection });
    }
}
