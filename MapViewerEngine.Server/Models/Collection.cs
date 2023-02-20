using System.ComponentModel.DataAnnotations;

namespace MapViewerEngine.Server.Models;

public class Collection
{
    public int Id { get; init; }

    [StringLength(255)]
    public required string Name { get; set; }
}
