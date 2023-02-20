namespace MapViewerEngine.Server.Models.Dtos;

public class OfficialBlockDto
{
    public required CollectionDto Collection { get; set; }
    public required AuthorDto Author { get; set; }
    public required string Name { get; set; }
    public required byte[] Meta { get; set; }
}
