using MapViewerEngine.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace MapViewerEngine.Server;

public interface IMapViewerEngineHub
{
    Task Meta(string blockName, string collection, string author);
    Task Metas(string[] blockNames, string collection, string author);
    string Ping();
}

[Authorize]
public class MapViewerEngineHub : Hub, IMapViewerEngineHub
{
    private readonly IMapViewerEngineUnitOfWork unitOfWork;

    public MapViewerEngineHub(IMapViewerEngineUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public string Ping()
    {
        return "Pong";
    }

    public async Task BlockMesh(BlockVariant block)
    {
        _ = block ?? throw new ArgumentNullException(nameof(block));

        var data = await unitOfWork.OfficialBlockMeshes.GetDataAsync(block);

        if (data is null)
        {
            // Shouldn't happen or subject to more intense rate limiting
            throw new Exception("Shouldn't happen or subject to more intense rate limiting");
        }

        await Clients.Caller.SendAsync("BlockMesh", block, data);
    }

    public async Task Meta(string blockName, string collection, string author)
    {
        _ = blockName ?? throw new ArgumentNullException(nameof(blockName));
        _ = collection ?? throw new ArgumentNullException(nameof(collection));
        _ = author ?? throw new ArgumentNullException(nameof(author));

        var data = await unitOfWork.OfficialBlocks.GetMetaByIdentAsync(blockName, collection, author);

        if (data is null)
        {
            // Shouldn't happen or subject to more intense rate limiting
            throw new Exception("Shouldn't happen or subject to more intense rate limiting");
        }
        
        await Clients.Caller.SendAsync("Meta", blockName, collection, data);
    }

    public async Task Metas(string[] blockNames, string collection, string author)
    {
        _ = blockNames ?? throw new ArgumentNullException(nameof(blockNames));
        _ = collection ?? throw new ArgumentNullException(nameof(collection));
        _ = author ?? throw new ArgumentNullException(nameof(author));

        if (blockNames.Length == 0 || blockNames.Length > 100)
        {
            throw new ArgumentException("Must be between 1 and 100 block names", nameof(blockNames));
        }

        var data = await unitOfWork.OfficialBlocks.GetMetasByMultipleIdentsAsync(blockNames, collection, author);

        if (!data.Any())
        {
            throw new Exception("Shouldn't happen or subject to more intense rate limiting");
        }

        await Clients.Caller.SendAsync("Metas", data);
    }
}
