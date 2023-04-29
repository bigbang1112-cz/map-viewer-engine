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
    public DbSet<OfficialShader> OfficialShaders { get; set; }
    public DbSet<OfficialScene> OfficialScenes { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }

    public MapViewerEngineContext(DbContextOptions<MapViewerEngineContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>().HasData(
            new Author { Id = 1, Name = "Nadeo" }
        );

        modelBuilder.Entity<Collection>().HasData(
            new Collection { Id = 1, Name = "Speed" },
            new Collection { Id = 2, Name = "Alpine" },
            new Collection { Id = 3, Name = "Rally" },
            new Collection { Id = 4, Name = "Island" },
            new Collection { Id = 5, Name = "Bay" },
            new Collection { Id = 6, Name = "Coast" },
            new Collection { Id = 7, Name = "Stadium" },
            new Collection { Id = 8, Name = "Canyon" },
            new Collection { Id = 9, Name = "StadiumMP4" },
            new Collection { Id = 10, Name = "Valley" },
            new Collection { Id = 11, Name = "Lagoon" },
            new Collection { Id = 12, Name = "Stadium2020" }
        );
    }
}
