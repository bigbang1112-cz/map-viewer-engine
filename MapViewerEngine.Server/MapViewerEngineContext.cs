using MapViewerEngine.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace MapViewerEngine.Server;

public class MapViewerEngineContext : DbContext
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<Collection> Collections { get; set; }
    public DbSet<Mesh> Meshes { get; set; }
    public DbSet<OfficialBlock> OfficialBlocks { get; set; }
    public DbSet<OfficialBlockMesh> OfficialBlockMeshes { get; set; }
    public DbSet<OfficialItemMesh> OfficialItemMeshes { get; set; }

    public MapViewerEngineContext(DbContextOptions<MapViewerEngineContext> options) : base(options)
    {
    }
}
