using GbxToolAPI.Server.Options;
using GbxToolAPI.Server;
using MapViewerEngine.Server.Models;
using Microsoft.Extensions.Options;

namespace MapViewerEngine.Server.Repos;

public interface IUserRequestRepo
{
    void Add(UserRequest request);
    Task<UserRequest> GetByRequestName(string requestName);
}

public class UserRequestRepo : IUserRequestRepo
{
    public UserRequestRepo(MapViewerEngineContext context, ISqlConnection<MapViewerEngineServer> sql, IOptions<DatabaseOptions> options)
    {

    }

    public void Add(UserRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<UserRequest> GetByRequestName(string requestName)
    {
        throw new NotImplementedException();
    }
}
