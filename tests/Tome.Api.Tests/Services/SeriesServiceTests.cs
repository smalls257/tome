using Moq;
using Tome.Api.Models;
using Tome.Api.Models.DTOs;
using Tome.Api.Repositories;
using Tome.Api.Services;

namespace Tome.Api.Tests.Services;

public class SeriesServiceTests
{
    private readonly Mock<ISeriesRepository> _mockRepo = new();
    private readonly SeriesService _service;

    public SeriesServiceTests()
    {
        _service = new SeriesService(_mockRepo.Object);
    }

    [Fact]
    public async Task PatchAsync_ValidIdWithAllFields_ReturnsUpdatedDto()
    {
        // Arrange
        var existing = new Series { Id = 1, Title = "Dune", Type = SeriesType.Book, Status = SeriesStatus.Ongoing, Monitored = true };
        var patch = new PatchSeriesDto { Title = "Dune Messiah", Type = SeriesType.Book, Status = SeriesStatus.Ended };
        var updated = new Series { Id = 1, Title = "Dune Messiah", Type = SeriesType.Book, Status = SeriesStatus.Ended, Monitored = true };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Series>())).ReturnsAsync(updated);

        // Act
        var result = await _service.PatchAsync(1, patch);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Dune Messiah", result.Title);
        Assert.Equal("Book", result.Type);
        Assert.Equal("Ended", result.Status);
    }

    [Fact]
    public async Task PatchAsync_InvalidId_ReturnsNull()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Series?)null);

        // Act
        var result = await _service.PatchAsync(99, new PatchSeriesDto { Title = "Ghost" });

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task PatchAsync_PartialFields_OnlyUpdatesProvidedFields()
    {
        // Arrange
        var existing = new Series { Id = 1, Title = "Dune", Type = SeriesType.Book, Status = SeriesStatus.Ongoing, Monitored = true };
        var patch = new PatchSeriesDto { Title = "Dune Messiah" }; // Title only — Type and Status omitted
        Series? captured = null;
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        _mockRepo
            .Setup(r => r.UpdateAsync(It.IsAny<Series>()))
            .Callback<Series>(s => captured = s)
            .ReturnsAsync((Series s) => s);

        // Act
        var result = await _service.PatchAsync(1, patch);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(captured);
        Assert.Equal("Dune Messiah", captured.Title);
        Assert.Equal(SeriesType.Book, captured.Type);       // unchanged
        Assert.Equal(SeriesStatus.Ongoing, captured.Status); // unchanged
    }

    [Fact]
    public async Task PatchAsync_EmptyPatch_ReturnsDtoWithOriginalValues()
    {
        // Arrange
        var existing = new Series { Id = 1, Title = "Saga", Type = SeriesType.Comic, Status = SeriesStatus.Ongoing, Monitored = false };
        var patch = new PatchSeriesDto(); // no fields set
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Series>())).ReturnsAsync((Series s) => s);

        // Act
        var result = await _service.PatchAsync(1, patch);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Saga", result.Title);
        Assert.Equal("Comic", result.Type);
        Assert.Equal("Ongoing", result.Status);
    }
}
