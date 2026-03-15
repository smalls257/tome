namespace Tome.Api.Models.DTOs;

public class SeriesDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool Monitored { get; set; }
}

public class CreateSeriesDto
{
    public required string Title { get; set; }
    public SeriesType Type { get; set; }
    public SeriesStatus Status { get; set; } = SeriesStatus.Ongoing;
    public bool Monitored { get; set; } = true;
}
