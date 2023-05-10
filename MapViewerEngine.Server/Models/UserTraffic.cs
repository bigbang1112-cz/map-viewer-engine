namespace MapViewerEngine.Server.Models;

public class UserTraffic
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public required User User { get; set; }

    public DateTimeOffset Timestamp { get; set; }

    public int RequestId { get; set; }
    public required UserRequest Request { get; set; }

    public required int Bytes { get; set; }
}
