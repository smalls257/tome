using System.Diagnostics;
using Tome.Api.Models.DTOs;

namespace Tome.Api.Services;

public class StatusService : IStatusService
{
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

    public StatusDto GetStatus() => new()
    {
        UptimeSeconds = (long)_stopwatch.Elapsed.TotalSeconds,
    };
}
