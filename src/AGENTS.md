# src/ — Agent Context

All application source code lives under `src/Tome.Api/`.

## Project Structure

```
src/Tome.Api/
├── Controllers/    HTTP layer — thin, no business logic
├── Services/       Business logic — interfaces + implementations
├── Repositories/   Data access — interfaces + implementations
├── Models/
│   ├── *.cs        Domain models (not exposed via API)
│   └── DTOs/       Data transfer objects (exposed via API)
└── Program.cs      DI registration and middleware
```

## Adding a New Entity

1. Add the domain model to `Models/`
2. Add the DTO(s) to `Models/DTOs/`
3. Add the repository interface and implementation to `Repositories/`
4. Add the service interface and implementation to `Services/`
5. Add the controller to `Controllers/`
6. Register the new service and repository in `Program.cs`

Follow the existing `Series` entity as the canonical pattern.

## See Also

- `src/Tome.Api/Controllers/AGENTS.md`
- `src/Tome.Api/Services/AGENTS.md`
- `src/Tome.Api/Repositories/AGENTS.md` (if present)
