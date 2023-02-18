using System.ComponentModel.DataAnnotations;

namespace MapViewerEngine.Server.Models;

public class OfficialItemMesh
{
    public int Id { get; private set; }
    
    [StringLength(255)]
    public required string Name { get; set; }

    public int CollectionId { get; set; }
    public required Collection Collection { get; set; }

    public int AuthorId { get; set; }
    public required Author Author { get; set; }
    
    public int MeshId { get; set; }
    public required Mesh Mesh { get; set; }
}
