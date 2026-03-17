using Tome.Api.Models.DTOs;

namespace Tome.Api.Services;

public interface IHealthService
{
    Task<HealthDto> GetHealthAsync();
}
