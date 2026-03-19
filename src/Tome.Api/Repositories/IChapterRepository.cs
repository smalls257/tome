using Tome.Api.Models;

namespace Tome.Api.Repositories;

public interface IChapterRepository
{
    Task<IEnumerable<Chapter>> GetBySeriesIdAsync(int seriesId);
}
