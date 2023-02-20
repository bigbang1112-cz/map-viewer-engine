using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace MapViewerEngine.Server.Models;

public class Mesh
{
    public int Id { get; private set; }
    public required Guid Guid { get; set; }

    [Column(TypeName = "mediumblob")]
    public required byte[] Data { get; set; }
}
