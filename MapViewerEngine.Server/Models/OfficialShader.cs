using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MapViewerEngine.Server.Models;

public class OfficialShader
{
    public int Id { get; private set; }

    [StringLength(255)]
    public required string Name { get; set; }

    [Column(TypeName = "blob")]
    public required byte[] Data { get; set; }
}
