using Tome.Api.Models;
using Tome.Api.Models.DTOs;

namespace Tome.Api.Services;

public interface ISeriesService
{
    Task<IEnumerable<SeriesDto>> GetAllAsync();
    Task<IEnumerable<SeriesDto>> GetByStatusAsync(SeriesStatus status);
    Task<SeriesDto?> GetByIdAsync(int id);
    Task<SeriesDto> CreateAsync(CreateSeriesDto dto);
}
