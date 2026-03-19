namespace Tome.Api.Models;

public class Chapter
{
    public int Id { get; set; }
    public int SeriesId { get; set; }
    public required string Title { get; set; }
    public decimal Number { get; set; }
}
