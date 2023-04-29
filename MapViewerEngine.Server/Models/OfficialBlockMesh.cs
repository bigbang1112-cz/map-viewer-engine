namespace MapViewerEngine.Server.Models;

public class OfficialBlockMesh
{
    public int Id { get; private set; }

    public int OfficialBlockId { get; set; }
    public required OfficialBlock OfficialBlock { get; set; }

    public bool Ground { get; set; }
    public byte Variant { get; set; }
    public byte SubVariant { get; set; }
    
    public int MeshId { get; set; }
    public required Mesh Mesh { get; set; }

    public float GeomTranslationX { get; set; }
    public float GeomTranslationY { get; set; }
    public float GeomTranslationZ { get; set; }
    public float GeomRotationX { get; set; }
    public float GeomRotationY { get; set; }
    public float GeomRotationZ { get; set; }
}
