namespace Tome.Api.Models;

public enum SeriesType { Book, Comic }
public enum SeriesStatus { Ongoing, Ended, Cancelled }

public class Series
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public SeriesType Type { get; set; }
    public SeriesStatus Status { get; set; }
    public bool Monitored { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
