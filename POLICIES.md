# Tome — Architectural Policies

These rules are checked by the semantic reviewer on every agent-generated PR.
Violations with confidence > 0.85 must be fixed before merge.

---

## P01 — No business logic in controllers

**Rule:** Controllers must not contain `if` statements, calculations, or data transformation beyond HTTP plumbing.

**Violation example:**
```csharp
// BAD — filtering in controller
public async Task<ActionResult<IEnumerable<SeriesDto>>> GetMonitored()
{
    var all = await _service.GetAllAsync();
    return Ok(all.Where(s => s.Monitored)); // logic belongs in service
}
```

**Fix:** Move the filter into a dedicated service method.

---

## P02 — Domain models must not cross the controller boundary

**Rule:** Controllers return DTOs only. Domain models (`Series`, etc.) must never appear in controller method signatures or return types.

**Violation example:**
```csharp
// BAD — returning domain model
public async Task<ActionResult<Series>> GetById(int id) { ... }
```

---

## P03 — No repository access in controllers

**Rule:** Controllers must only inject service interfaces, never repository interfaces.

**Violation example:**
```csharp
// BAD
public class SeriesController(ISeriesRepository repo) : ControllerBase { ... }
```

---

## P04 — No AutoMapper

**Rule:** Use explicit `ToDto()` static methods in services. AutoMapper is not used in this project.

---

## P05 — Interfaces required for services and repositories

**Rule:** Every service and repository must have a corresponding interface. DI must be registered against the interface, not the concrete type.

**Violation example:**
```csharp
// BAD
builder.Services.AddScoped<SeriesService>(); // should be AddScoped<ISeriesService, SeriesService>()
```

---

## P06 — Repository implements data access only

**Rule:** Repositories must not contain business logic, validation, or DTO mapping. They work with domain models only.
