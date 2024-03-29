﻿using GbxToolAPI.Server;
using GbxToolAPI.Server.Options;
using MapViewerEngine.Server.Repos;
using Microsoft.Extensions.Options;
using System.Data;

namespace MapViewerEngine.Server;

public interface IMapViewerEngineUnitOfWork
{
    ICollectionRepo Collections { get; }
    IMeshRepo Meshes { get; }
    IOfficialBlockMeshRepo OfficialBlockMeshes { get; }
    IOfficialBlockRepo OfficialBlocks { get; }
    IOfficialShaderRepo OfficialShaders { get; }
    IOfficialSceneRepo OfficialScenes { get; }
    IVehicleRepo Vehicles { get; }

    void Save();
    Task SaveAsync(CancellationToken cancellationToken = default);
}

public class MapViewerEngineUnitOfWork : IMapViewerEngineUnitOfWork
{
    private readonly MapViewerEngineContext context;

    public ICollectionRepo Collections { get; }
    public IMeshRepo Meshes { get; }
    public IOfficialBlockMeshRepo OfficialBlockMeshes { get; }
    public IOfficialBlockRepo OfficialBlocks { get; }
    public IOfficialShaderRepo OfficialShaders { get; }
    public IOfficialSceneRepo OfficialScenes { get; }
    public IVehicleRepo Vehicles { get; }

    public MapViewerEngineUnitOfWork(MapViewerEngineContext context, ISqlConnection<MapViewerEngineServer> sql, IOptions<DatabaseOptions> databaseOptions)
    {
        this.context = context;

        Collections = new CollectionRepo(context, sql, databaseOptions);
        Meshes = new MeshRepo(context, sql, databaseOptions);
        OfficialBlockMeshes = new OfficialBlockMeshRepo(context, sql, databaseOptions);
        OfficialBlocks = new OfficialBlockRepo(context, sql, databaseOptions);
        OfficialShaders = new OfficialShaderRepo(context, sql, databaseOptions);
        OfficialScenes = new OfficialSceneRepo(context, sql, databaseOptions);
        Vehicles = new VehicleRepo(context, sql, databaseOptions);
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
