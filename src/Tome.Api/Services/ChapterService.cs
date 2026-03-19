using Tome.Api.Models;
using Tome.Api.Models.DTOs;
using Tome.Api.Repositories;

namespace Tome.Api.Services;

public class ChapterService(ISeriesRepository seriesRepository, IChapterRepository chapterRepository) : IChapterService
{
    public async Task<IEnumerable<ChapterDto>?> GetBySeriesIdAsync(int seriesId)
    {
        var series = await seriesRepository.GetByIdAsync(seriesId);
        if (series is null) return null;

        var chapters = await chapterRepository.GetBySeriesIdAsync(seriesId);
        return chapters.Select(ToDto);
    }

    private static ChapterDto ToDto(Chapter c) => new()
    {
        Id = c.Id,
        SeriesId = c.SeriesId,
        Title = c.Title,
        Number = c.Number,
    };
}
