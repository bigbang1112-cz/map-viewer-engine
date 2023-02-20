using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MapViewerEngine.Server.Models;

public class OfficialBlock
{
    public int Id { get; private set; }
    
    public int CollectionId { get; set; }
    public required Collection Collection { get; set; }
    
    public int AuthorId { get; set; }
    public required Author Author { get; set; }
    
    [StringLength(255)]
    public required string Name { get; set; }
    
    [Column(TypeName = "blob")]
    public required byte[] Meta { get; set; }

    [JsonIgnore] public ICollection<OfficialBlockMesh> OfficialBlockMeshes { get; set; } = Array.Empty<OfficialBlockMesh>();
}
