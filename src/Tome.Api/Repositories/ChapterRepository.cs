using Tome.Api.Models;

namespace Tome.Api.Repositories;

/// <summary>
/// In-memory repository — swap for EF Core in production.
/// </summary>
public class ChapterRepository : IChapterRepository
{
    private readonly List<Chapter> _store = [];

    public Task<IEnumerable<Chapter>> GetBySeriesIdAsync(int seriesId) =>
        Task.FromResult<IEnumerable<Chapter>>(_store.Where(c => c.SeriesId == seriesId).ToList());
}
