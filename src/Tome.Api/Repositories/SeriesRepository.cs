using Tome.Api.Models;

namespace Tome.Api.Repositories;

/// <summary>
/// In-memory repository — swap for EF Core in production.
/// </summary>
public class SeriesRepository : ISeriesRepository
{
    private readonly List<Series> _store =
    [
        new() { Id = 1, Title = "Dune", Type = SeriesType.Book, Status = SeriesStatus.Ended, Monitored = true },
        new() { Id = 2, Title = "Saga", Type = SeriesType.Comic, Status = SeriesStatus.Ongoing, Monitored = true },
    ];

    public Task<IEnumerable<Series>> GetAllAsync() =>
        Task.FromResult<IEnumerable<Series>>(_store);

    public Task<Series?> GetByIdAsync(int id) =>
        Task.FromResult(_store.FirstOrDefault(s => s.Id == id));

    public Task<Series> AddAsync(Series series)
    {
        series.Id = _store.Count > 0 ? _store.Max(s => s.Id) + 1 : 1;
        _store.Add(series);
        return Task.FromResult(series);
    }
}
