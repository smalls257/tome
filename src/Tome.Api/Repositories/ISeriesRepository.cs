using Tome.Api.Models;

namespace Tome.Api.Repositories;

public interface ISeriesRepository
{
    Task<IEnumerable<Series>> GetAllAsync();
    Task<IEnumerable<Series>> GetMonitoredAsync();
    Task<Series?> GetByIdAsync(int id);
    Task<Series> AddAsync(Series series);
}
