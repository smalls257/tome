# Services — Agent Context

Services contain all business logic. They sit between controllers and repositories.

## Rules

- Always define an interface (`ISeriesService`) alongside the implementation (`SeriesService`).
- Map domain models to DTOs in the service — not in controllers, not in repositories.
- Services may call multiple repositories if needed.
- Services must not access `HttpContext` or anything HTTP-related.

## Canonical Pattern

```csharp
public class SeriesService(ISeriesRepository repository) : ISeriesService
{
    public async Task<IEnumerable<SeriesDto>> GetAllAsync()
    {
        var series = await repository.GetAllAsync();
        return series.Select(ToDto);
    }

    private static SeriesDto ToDto(Series s) => new()
    {
        Id = s.Id,
        Title = s.Title,
        Type = s.Type.ToString(),
        Status = s.Status.ToString(),
        Monitored = s.Monitored,
    };
}
```

## Mapping

- Use a private static `ToDto()` method per entity.
- Never use AutoMapper — explicit mapping only.

## Reference File

`src/Tome.Api/Services/SeriesService.cs`
