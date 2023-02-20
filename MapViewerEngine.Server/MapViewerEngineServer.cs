using GbxToolAPI.Server;
using MapViewerEngine.Server.Repos;
using Microsoft.Extensions.DependencyInjection;

namespace MapViewerEngine.Server;

public class MapViewerEngineServer : IServer
{
    public string ConnectionString => "MapViewerEngine";

    public static void Services(IServiceCollection services)
    {
        services.AddScoped<IOfficialBlockMeshRepo, OfficialBlockMeshRepo>();
        services.AddScoped<IOfficialBlockRepo, OfficialBlockRepo>();
        services.AddScoped<ICollectionRepo, CollectionRepo>();
        services.AddScoped<IMeshRepo, MeshRepo>();
        services.AddScoped<IMapViewerEngineUnitOfWork, MapViewerEngineUnitOfWork>();
    }
}
