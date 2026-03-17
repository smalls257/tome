namespace Tome.Api.Models.DTOs;

public class HealthDto
{
    public string Status { get; set; } = string.Empty;
    public DateTimeOffset Timestamp { get; set; }
}
