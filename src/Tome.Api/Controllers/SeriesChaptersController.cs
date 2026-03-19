using Microsoft.AspNetCore.Mvc;
using Tome.Api.Models.DTOs;
using Tome.Api.Services;

namespace Tome.Api.Controllers;

[ApiController]
[Route("api/series")]
public class SeriesChaptersController(IChapterService chapterService) : ControllerBase
{
    [HttpGet("{id:int}/chapters")]
    public async Task<ActionResult<IEnumerable<ChapterDto>>> GetChapters(int id)
    {
        var chapters = await chapterService.GetBySeriesIdAsync(id);
        return chapters is null ? NotFound() : Ok(chapters);
    }
}
