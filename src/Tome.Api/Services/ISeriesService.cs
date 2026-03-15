using Tome.Api.Models.DTOs;

namespace Tome.Api.Services;

public interface ISeriesService
{
    Task<IEnumerable<SeriesDto>> GetAllAsync();
    Task<SeriesDto?> GetByIdAsync(int id);
    Task<SeriesDto> CreateAsync(CreateSeriesDto dto);
}
