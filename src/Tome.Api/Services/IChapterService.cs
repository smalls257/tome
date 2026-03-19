using Tome.Api.Models.DTOs;

namespace Tome.Api.Services;

public interface IChapterService
{
    Task<IEnumerable<ChapterDto>?> GetBySeriesIdAsync(int seriesId);
}
