namespace Tome.Api.Models.DTOs;

public class ChapterDto
{
    public int Id { get; set; }
    public int SeriesId { get; set; }
    public required string Title { get; set; }
    public decimal Number { get; set; }
}
