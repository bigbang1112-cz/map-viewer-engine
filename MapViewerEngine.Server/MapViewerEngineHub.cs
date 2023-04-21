using MapViewerEngine.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;

namespace MapViewerEngine.Server;

public interface IMapViewerEngineHub
{
    Task Meta(string blockName, string collection, string author);
    Task Metas(string[] blockNames, string collection, string author);
    string Ping();
}

//[Authorize]
public class MapViewerEngineHub : Hub, IMapViewerEngineHub
{
    private readonly IMapViewerEngineUnitOfWork unitOfWork;
    private readonly IMemoryCache cache;

    public MapViewerEngineHub(IMapViewerEngineUnitOfWork unitOfWork, IMemoryCache cache)
    {
        this.unitOfWork = unitOfWork;
        this.cache = cache;
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

        var cacheKey = $"Meta:{blockName}:{collection}:{author}";

        if (cache.TryGetValue(cacheKey, out byte[]? blockMeta) && blockMeta is not null)
        {
            await Clients.Caller.SendAsync("Meta", blockName, collection, blockMeta);
            return;
        }

        var data = await unitOfWork.OfficialBlocks.GetMetaByIdentAsync(blockName, collection, author);

        if (data is null)
        {
            // Shouldn't happen or subject to more intense rate limiting
            throw new Exception("Shouldn't happen or subject to more intense rate limiting");
        }

        cache.Set(cacheKey, data, TimeSpan.FromHours(1));
        
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

        if (blockNames.Length == 1)
        {
            await Meta(blockNames[0], collection, author);
            return;
        }

        var metasToRequest = new List<string>();
        var data = new List<OfficialBlockMeta>();

        foreach (var blockName in blockNames)
        {
            if (string.IsNullOrWhiteSpace(blockName))
            {
                throw new ArgumentException("Block name cannot be null or whitespace", nameof(blockNames));
            }

            if (cache.TryGetValue($"Meta:{blockName}:{collection}:{author}", out byte[]? blockMeta) && blockMeta is not null)
            {
                data.Add(new OfficialBlockMeta() { Name = blockName, Meta = blockMeta });
            }
            else
            {
                metasToRequest.Add(blockName);
            }
        }

        foreach (var meta in await unitOfWork.OfficialBlocks.GetMetasByMultipleIdentsAsync(metasToRequest, collection, author))
        {
            data.Add(meta);
            cache.Set($"Meta:{meta.Name}:{collection}:{author}", meta.Meta, TimeSpan.FromHours(1));
        }

        if (data.Count == 0)
        {
            throw new Exception("Shouldn't happen or subject to more intense rate limiting");
        }

        await Clients.Caller.SendAsync("Metas", data);
    }

    public async Task Shader(string shaderName)
    {
        _ = shaderName ?? throw new ArgumentNullException(nameof(shaderName));

        var cacheKey = $"Shader:{shaderName}";

        if (cache.TryGetValue(cacheKey, out byte[]? data) && data is not null)
        {
            await Clients.Caller.SendAsync("Shader", shaderName, data);
            return;
        }

        data = await unitOfWork.OfficialShaders.GetByNameAsync(shaderName);

        if (data is null)
        {
            // Shouldn't happen or subject to more intense rate limiting
            throw new Exception($"Shouldn't happen or subject to more intense rate limiting ({shaderName})");
        }

        cache.Set(cacheKey, data, TimeSpan.FromHours(1));

        await Clients.Caller.SendAsync("Shader", shaderName, data);
    }
}
