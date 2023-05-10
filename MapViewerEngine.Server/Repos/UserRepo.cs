using GbxToolAPI.Server.Options;
using GbxToolAPI.Server;
using MapViewerEngine.Server.Models;
using Microsoft.Extensions.Options;

namespace MapViewerEngine.Server.Repos;

public interface IUserRepo
{
    void Add(User user);
    Task<User> GetBySnowflake(ulong snowflake);
}

public class UserRepo : IUserRepo
{
    public UserRepo(MapViewerEngineContext context, ISqlConnection<MapViewerEngineServer> sql, IOptions<DatabaseOptions> options)
    {
        
    }

    public void Add(User user)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetBySnowflake(ulong snowflake)
    {
        throw new NotImplementedException();
    }
}
