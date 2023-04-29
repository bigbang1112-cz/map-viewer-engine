using System.ComponentModel.DataAnnotations;

namespace MapViewerEngine.Server.Models;

public class Vehicle
{
    public int Id { get; private set; }

    [StringLength(255)]
    public required string Name { get; set; }

    public int MeshId { get; set; }
    public required Mesh Mesh { get; set; }
}
