# Controllers — Agent Context

Controllers are the HTTP layer only. They must be thin.

## Rules

- Inject the service interface via the primary constructor — never the repository directly.
- Return `ActionResult<T>` for typed responses.
- Use `Ok()`, `NotFound()`, `CreatedAtAction()`, `BadRequest()` — no raw status codes.
- Do not contain business logic. If you find yourself writing an `if` that isn't about HTTP status, move it to the service.
- Do not map domain models to DTOs here — that's the service's job.

## Canonical Pattern

```csharp
[ApiController]
[Route("api/[controller]")]
public class SeriesController(ISeriesService seriesService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SeriesDto>>> GetAll() =>
        Ok(await seriesService.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SeriesDto>> GetById(int id)
    {
        var series = await seriesService.GetByIdAsync(id);
        return series is null ? NotFound() : Ok(series);
    }
}
```

## Route Conventions

- Collection: `GET /api/series`
- Single item: `GET /api/series/{id:int}`
- Create: `POST /api/series`
- Custom action: `GET /api/series/{id:int}/missing` (noun-based, not verb-based)

## Reference File

`src/Tome.Api/Controllers/SeriesController.cs`
