using GbxToolAPI.Server;
using MapViewerEngine.Server.Repos;
using Microsoft.Extensions.DependencyInjection;

namespace MapViewerEngine.Server;

public class MapViewerEngineServer : IServer
{
    public void Services(IServiceCollection services)
    {
        services.AddScoped<IOfficialBlockMeshRepo, OfficialBlockMeshRepo>();
        services.AddScoped<IMeshRepo, MeshRepo>();
    }
}
