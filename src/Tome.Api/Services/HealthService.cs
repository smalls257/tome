using System.Reflection;
using Tome.Api.Models.DTOs;

namespace Tome.Api.Services;

public class HealthService : IHealthService
{
    public Task<HealthDto> GetHealthAsync()
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "unknown";
        return Task.FromResult(new HealthDto
        {
            Status = "Healthy",
            Version = version,
        });
    }
}
