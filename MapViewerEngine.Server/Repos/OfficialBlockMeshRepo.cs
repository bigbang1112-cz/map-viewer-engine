﻿using Dapper;
using GbxToolAPI.Server;
using GbxToolAPI.Server.Options;
using MapViewerEngine.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;

namespace MapViewerEngine.Server.Repos;

public interface IOfficialBlockMeshRepo
{
    Task<BlockData?> GetDataAsync(BlockVariant block, CancellationToken cancellationToken = default);
}

public class OfficialBlockMeshRepo : IOfficialBlockMeshRepo
{
    private readonly MapViewerEngineContext context;
    private readonly ISqlConnection<MapViewerEngineServer> sql;
    private readonly IOptions<DatabaseOptions> options;

    public OfficialBlockMeshRepo(MapViewerEngineContext context, ISqlConnection<MapViewerEngineServer> sql, IOptions<DatabaseOptions> options)
    {
        this.context = context;
        this.sql = sql;
        this.options = options;
    }

    private bool IsInMemory => options.Value.InMemory;

    public async Task<BlockData?> GetDataAsync(BlockVariant block, CancellationToken cancellationToken = default)
    {
        _ = block ?? throw new ArgumentNullException(nameof(block));

        if (IsInMemory)
        {
            return await context.OfficialBlockMeshes
                .Include(bm => bm.Mesh)
                .Include(bm => bm.OfficialBlock)
                .Where(bm => bm.OfficialBlock.Name == block.Name && bm.OfficialBlock.CollectionId == block.CollectionId && bm.Ground == block.Ground && bm.Variant == block.Variant && bm.SubVariant == block.SubVariant)
                .Select(bm => new BlockData(bm.Mesh.Data, bm.GeomTranslationX, bm.GeomTranslationY, bm.GeomTranslationZ, bm.GeomRotationX, bm.GeomRotationY, bm.GeomRotationZ))
                .FirstOrDefaultAsync(cancellationToken);
        }

        return await sql.Connection.QueryFirstOrDefaultAsync<BlockData?>(
            @"SELECT m.Data, bm.GeomTranslationX, bm.GeomTranslationY, bm.GeomTranslationZ, bm.GeomRotationX, bm.GeomRotationY, bm.GeomRotationZ FROM OfficialBlockMeshes bm
            INNER JOIN Meshes m ON bm.MeshId = m.id
            INNER JOIN OfficialBlocks ob ON bm.OfficialBlockId = ob.id
            WHERE ob.name = @Name AND ob.CollectionId = @CollectionId AND bm.Ground = @Ground AND bm.Variant = @Variant AND bm.SubVariant = @SubVariant",
            new { block.Name, block.CollectionId, block.Ground, block.Variant, block.SubVariant });
    }
}
