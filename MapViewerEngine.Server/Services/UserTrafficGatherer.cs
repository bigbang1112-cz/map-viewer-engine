using MapViewerEngine.Server.Models;

namespace MapViewerEngine.Server.Services;

public class UserTrafficGatherer
{
    private readonly MapViewerEngineUnitOfWork _uow;

    public UserTrafficGatherer(MapViewerEngineUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task AddUserTrafficAsync(ulong snowflake, string requestName, int bytes)
    {
        var user = await _uow.Users.GetBySnowflake(snowflake);

        if (user is null)
        {
            user = new User
            {
                Snowflake = snowflake
            };

            _uow.Users.Add(user);
        }

        var request = await _uow.UserRequests.GetByRequestName(requestName);
        
        if (request is null)
        {
            request = new UserRequest
            {
                Name = requestName
            };
            _uow.UserRequests.Add(request);
        }

        var traffic = new UserTraffic
        {
            User = user,
            Request = request,
            Bytes = bytes,
            Timestamp = DateTimeOffset.UtcNow
        };

        _uow.UserTraffic.Add(traffic);

        await _uow.SaveAsync();
    }
}
