namespace MapViewerEngine.Server.Models;

public class Mesh
{
    public int Id { get; private set; }
    public Guid Guid { get; set; }
    public required byte[] Data { get; set; }

    public required ICollection<OfficialBlockMesh> OfficialBlockMeshes { get; set; }
    public required ICollection<OfficialItemMesh> OfficialItemMeshes { get; set; }
}
