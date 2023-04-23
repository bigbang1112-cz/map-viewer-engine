namespace MapViewerEngine.Server.Models;

public class OfficialScene
{
    public int Id { get; private set; }
    
    public int CollectionId { get; set; }
    public required Collection Collection { get; set; }

    public required int SizeX { get; set; }
    public required int SizeZ { get; set; }

    public int MeshId { get; set; }
    public required Mesh Mesh { get; set; }
}
