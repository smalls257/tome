using Tome.Api.Models;
using Tome.Api.Models.DTOs;
using Tome.Api.Repositories;

namespace Tome.Api.Services;

public class SeriesService(ISeriesRepository repository) : ISeriesService
{
    public async Task<IEnumerable<SeriesDto>> GetAllAsync()
    {
        var series = await repository.GetAllAsync();
        return series.Select(ToDto);
    }

    public async Task<SeriesDto?> GetByIdAsync(int id)
    {
        var series = await repository.GetByIdAsync(id);
        return series is null ? null : ToDto(series);
    }

    public async Task<SeriesDto> CreateAsync(CreateSeriesDto dto)
    {
        var series = new Series
        {
            Title = dto.Title,
            Type = dto.Type,
            Status = dto.Status,
            Monitored = dto.Monitored,
        };
        var created = await repository.AddAsync(series);
        return ToDto(created);
    }

    public async Task<bool> DeleteAsync(int id) =>
        await repository.DeleteAsync(id);

    private static SeriesDto ToDto(Series s) => new()
    {
        Id = s.Id,
        Title = s.Title,
        Type = s.Type.ToString(),
        Status = s.Status.ToString(),
        Monitored = s.Monitored,
    };
}
