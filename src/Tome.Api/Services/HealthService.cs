using Tome.Api.Models.DTOs;

namespace Tome.Api.Services;

public class HealthService : IHealthService
{
    public Task<HealthDto> GetAsync() =>
        Task.FromResult(new HealthDto
        {
            Status = "ok",
            Timestamp = DateTimeOffset.UtcNow,
        });
}
