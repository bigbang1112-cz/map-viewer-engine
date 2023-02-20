using Dapper;
using GbxToolAPI.Server;
using GbxToolAPI.Server.Options;
using MapViewerEngine.Server.Models;
using MapViewerEngine.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MapViewerEngine.Server.Repos;

public interface IOfficialBlockRepo
{
    Task<IEnumerable<OfficialBlock>> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<OfficialBlock>> GetByCollectionAndNameAsync(string collection, string name, CancellationToken cancellationToken = default);
    Task<OfficialBlock?> GetByIdentAsync(string author, string collection, string name, CancellationToken cancellationToken = default);
    Task<byte[]?> GetMetaByIdentAsync(string name, string collection, string author, CancellationToken cancellationToken = default);
    Task<IEnumerable<OfficialBlockMeta>> GetMetasByMultipleIdentsAsync(string[] blockNames, string collection, string author, CancellationToken cancellationToken = default);
}

public class OfficialBlockRepo : IOfficialBlockRepo
{
    private readonly MapViewerEngineContext context;
    private readonly ISqlConnection<MapViewerEngineServer> sql;
    private readonly IOptions<DatabaseOptions> options;

    public OfficialBlockRepo(MapViewerEngineContext context, ISqlConnection<MapViewerEngineServer> sql, IOptions<DatabaseOptions> options)
    {
        this.context = context;
        this.sql = sql;
        this.options = options;
    }

    private bool IsInMemory => options.Value.InMemory;

    public async Task<IEnumerable<OfficialBlock>> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        if (IsInMemory)
        {
            return await context.OfficialBlocks
                .Where(b => b.Name == name)
                .ToListAsync(cancellationToken);
        }

        return await sql.Connection.QueryAsync<OfficialBlock>("SELECT * FROM OfficialBlocks WHERE Name = @Name", new { Name = name });
    }

    public async Task<IEnumerable<OfficialBlock>> GetByCollectionAndNameAsync(string collection, string name, CancellationToken cancellationToken = default)
    {
        if (IsInMemory)
        {
            return await context.OfficialBlocks
                .Include(b => b.Collection)
                .Where(b => b.Collection.Name == collection && b.Name == name)
                .ToListAsync(cancellationToken);
        }
        
        return await sql.Connection.QueryAsync<OfficialBlock>(
            @"SELECT b.* 
            FROM OfficialBlocks b 
            JOIN Collections c ON c.Id = b.CollectionId 
            WHERE c.Name = @Collection AND b.Name = @Name",
            new { Collection = collection, Name = name });

    }

    public async Task<OfficialBlock?> GetByIdentAsync(string author, string collection, string name, CancellationToken cancellationToken = default)
    {
        if (IsInMemory)
        {
            return await context.OfficialBlocks
                .Include(b => b.Author)
                .Include(b => b.Collection)
                .Where(b => b.Author.Name == author && b.Collection.Name == collection && b.Name == name)
                .FirstOrDefaultAsync(cancellationToken);
        }
        
        return (await sql.Connection.QueryAsync<OfficialBlock, Author, Collection, OfficialBlock?>(
            @"SELECT * FROM OfficialBlocks b
            JOIN Authors a ON a.Id = b.AuthorId
            JOIN Collections c ON c.Id = b.CollectionId
            WHERE a.Name = @Author AND c.Name = @Collection AND b.Name = @Name",
            (officialBlock, author, collection) =>
            {
                officialBlock.Author = author;
                officialBlock.Collection = collection;
                return officialBlock;
            },
            new { Author = author, Collection = collection, Name = name })).FirstOrDefault();
    }

    public async Task<byte[]?> GetMetaByIdentAsync(string name, string collection, string author, CancellationToken cancellationToken = default)
    {
        if (IsInMemory)
        {
            return await context.OfficialBlocks
                .Include(b => b.Author)
                .Include(b => b.Collection)
                .Where(b => b.Author.Name == author && b.Collection.Name == collection && b.Name == name)
                .Select(x => x.Meta)
                .FirstOrDefaultAsync(cancellationToken);
        }

        return await sql.Connection.QueryFirstOrDefaultAsync<byte[]?>(
            @"SELECT b.Meta FROM OfficialBlocks b
            JOIN Authors a ON a.Id = b.AuthorId
            JOIN Collections c ON c.Id = b.CollectionId
            WHERE a.Name = @Author AND c.Name = @Collection AND b.Name = @Name",
            new { Author = author, Collection = collection, Name = name });
    }

    public async Task<IEnumerable<OfficialBlockMeta>> GetMetasByMultipleIdentsAsync(string[] names, string collection, string author, CancellationToken cancellationToken = default)
    {
        if (IsInMemory)
        {
            return await context.OfficialBlocks
                .Include(b => b.Author)
                .Include(b => b.Collection)
                .Where(b => b.Author.Name == author && b.Collection.Name == collection && names.Contains(b.Name))
                .Select(x => new OfficialBlockMeta { Name = x.Name, Meta = x.Meta })
                .ToListAsync(cancellationToken);
        }

        return await sql.Connection.QueryAsync<OfficialBlockMeta>(
            @"SELECT b.Name, b.Meta FROM OfficialBlocks b
            JOIN Authors a ON a.Id = b.AuthorId
            JOIN Collections c ON c.Id = b.CollectionId
            WHERE a.Name = @Author AND c.Name = @Collection AND b.Name IN @Names",
            new { Author = author, Collection = collection, Names = names });
    }
}
