using GbxToolAPI.Server;
using Microsoft.Extensions.DependencyInjection;

namespace MapViewerEngine.Server;

public class MapViewerEngineServer : IServer
{
    public void Services(IServiceCollection services)
    {
        services.AddScoped<IMapViewerEngineRepo, MapViewerEngineRepo>();
    }
}
