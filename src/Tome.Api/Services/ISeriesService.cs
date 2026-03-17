using Tome.Api.Models.DTOs;

namespace Tome.Api.Services;

public interface ISeriesService
{
    Task<IEnumerable<SeriesDto>> GetAllAsync();
    Task<IEnumerable<SeriesDto>> GetMonitoredAsync();
    Task<SeriesDto?> GetByIdAsync(int id);
    Task<SeriesDto> CreateAsync(CreateSeriesDto dto);
}
