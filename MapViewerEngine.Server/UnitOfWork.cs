using GbxToolAPI.Server.Options;
using MapViewerEngine.Server.Repos;
using Microsoft.Extensions.Options;
using System.Data;

namespace MapViewerEngine.Server;

public interface IUnitOfWork
{
    ICollectionRepo Collections { get; }
    IMeshRepo Meshes { get; }
    IOfficialBlockMeshRepo OfficialBlockMeshes { get; }
    IOfficialBlockRepo OfficialBlocks { get; }

    void Save();
    Task SaveAsync(CancellationToken cancellationToken = default);
}

public class UnitOfWork : IUnitOfWork
{
    private readonly MapViewerEngineContext context;

    public ICollectionRepo Collections { get; }
    public IMeshRepo Meshes { get; }
    public IOfficialBlockMeshRepo OfficialBlockMeshes { get; }
    public IOfficialBlockRepo OfficialBlocks { get; }

    public UnitOfWork(MapViewerEngineContext context, IOptions<DatabaseOptions> databaseOptions, IDbConnection dbConnection)
    {
        this.context = context;

        Collections = new CollectionRepo(context, dbConnection, databaseOptions);
        Meshes = new MeshRepo(context, dbConnection, databaseOptions);
        OfficialBlockMeshes = new OfficialBlockMeshRepo(context, dbConnection, databaseOptions);
        OfficialBlocks = new OfficialBlockRepo(context, dbConnection, databaseOptions);
    }

    public void Save()
    {
        context.SaveChanges();
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}
