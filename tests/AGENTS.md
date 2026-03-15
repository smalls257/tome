# tests/ — Agent Context

All tests live under `tests/Tome.Api.Tests/`.

## Structure

Mirror the `src/Tome.Api/` structure:

```
tests/Tome.Api.Tests/
└── Controllers/    Unit tests for controllers
    └── {Entity}ControllerTests.cs
```

As more layers are tested, add:
- `Services/` for service unit tests
- `Repositories/` for repository tests (if applicable)

## Conventions

- Framework: xUnit
- Mocking: Moq
- Naming: `{Method}_{Scenario}_{ExpectedResult}`
- Structure: Arrange / Act / Assert with comments

## Canonical Pattern

```csharp
[Fact]
public async Task GetById_ValidId_ReturnsSeries()
{
    // Arrange
    var dto = new SeriesDto { Id = 1, Title = "Dune", ... };
    _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(dto);

    // Act
    var result = await _controller.GetById(1);

    // Assert
    var ok = Assert.IsType<OkObjectResult>(result.Result);
    var returned = Assert.IsType<SeriesDto>(ok.Value);
    Assert.Equal("Dune", returned.Title);
}
```

## What to Test

- Controller: mock the service, test HTTP response types and status codes only.
- Every new public endpoint needs: happy path + primary error case (e.g. NotFound).
- Do not test the repository through controller tests — mock the service boundary.

## Reference File

`tests/Tome.Api.Tests/Controllers/SeriesControllerTests.cs`
