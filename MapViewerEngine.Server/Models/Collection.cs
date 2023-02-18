using System.ComponentModel.DataAnnotations;

namespace MapViewerEngine.Server.Models;

public class Collection
{
    public int Id { get; private set; }
    public int Index { get; set; }
    
    [StringLength(255)]
    public required string Name { get; set; }
    
    public required ICollection<OfficialBlockMesh> OfficialBlockMeshes { get; set; }
    public required ICollection<OfficialItemMesh> OfficialItemMeshes { get; set; }
}
