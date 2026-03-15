# Tome — Agent Context

Tome is a .NET 9 Web API for tracking books and comics, inspired by the *arr ecosystem.
Users monitor series and track which items they have vs. are missing.

## Architecture

```
Controllers  →  Services  →  Repositories  →  (data store)
     ↓               ↓
   DTOs          Domain Models
```

- **Controllers** receive HTTP requests, validate input, return HTTP responses. No business logic.
- **Services** contain all business logic. Call repositories, map domain models to DTOs.
- **Repositories** handle data access only. No business logic.
- **DTOs** cross the controller boundary. Domain models never leave the service layer.

## Key Domain Concepts

- **Series** — a book series or comic run. Has a `Type` (Book or Comic) and a `Status` (Ongoing, Ended, Cancelled).
- **Monitored** — a flag indicating the user wants to track this series for new items.
- More entities (Item, Creator, Library) will be added by the factory.

## Dependency Injection

All services and repositories are registered in `Program.cs`:
- Repositories: `AddSingleton` (in-memory store is shared)
- Services: `AddScoped`

When adding a new service or repository, register it in `Program.cs` following this pattern.

## Naming

- Controllers: `{Entity}Controller`
- Services: `I{Entity}Service` + `{Entity}Service`
- Repositories: `I{Entity}Repository` + `{Entity}Repository`
- DTOs: `{Entity}Dto` (read), `Create{Entity}Dto` (write)

## See Also

- `src/AGENTS.md` — source layout details
- `POLICIES.md` — architectural rules enforced at review
