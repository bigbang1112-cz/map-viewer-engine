using GbxToolAPI.Server.Options;
using GbxToolAPI.Server;
using MapViewerEngine.Server.Models;
using Microsoft.Extensions.Options;

namespace MapViewerEngine.Server.Repos;

public interface IUserTrafficRepo
{
    void Add(UserTraffic traffic);
}

public class UserTrafficRepo : IUserTrafficRepo
{
    public UserTrafficRepo(MapViewerEngineContext context, ISqlConnection<MapViewerEngineServer> sql, IOptions<DatabaseOptions> options)
    {
        
    }

    public void Add(UserTraffic traffic)
    {
        throw new NotImplementedException();
    }
}
