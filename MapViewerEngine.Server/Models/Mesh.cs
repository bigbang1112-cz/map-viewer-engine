using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MapViewerEngine.Server.Models;

public class Mesh
{
    public int Id { get; private set; }
    public required Guid Guid { get; set; }

    [Column(TypeName = "mediumblob")]
    public required byte[] Data { get; set; }

    [JsonIgnore] public ICollection<OfficialBlockMesh> OfficialBlockMeshes { get; set; } = Array.Empty<OfficialBlockMesh>();
    [JsonIgnore] public ICollection<OfficialItemMesh> OfficialItemMeshes { get; set; } = Array.Empty<OfficialItemMesh>();
}
