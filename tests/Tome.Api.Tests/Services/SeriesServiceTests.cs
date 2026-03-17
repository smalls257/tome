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
    public async Task PatchAsync_ExistingId_ReturnsUpdatedSeriesDto()
    {
        // Arrange
        var existing = new Series { Id = 1, Title = "Dune", Type = SeriesType.Book, Status = SeriesStatus.Ongoing, Monitored = true };
        var patchDto = new PatchSeriesDto { Title = "Dune Messiah", Status = SeriesStatus.Ended };
        var afterUpdate = new Series { Id = 1, Title = "Dune Messiah", Type = SeriesType.Book, Status = SeriesStatus.Ended, Monitored = true };

        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Series>())).ReturnsAsync(afterUpdate);

        // Act
        var result = await _service.PatchAsync(1, patchDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Dune Messiah", result.Title);
        Assert.Equal("Book", result.Type);
        Assert.Equal("Ended", result.Status);
    }

    [Fact]
    public async Task PatchAsync_ExistingId_OnlyPatchesProvidedFields()
    {
        // Arrange
        var existing = new Series { Id = 2, Title = "Saga", Type = SeriesType.Comic, Status = SeriesStatus.Ongoing, Monitored = true };
        var patchDto = new PatchSeriesDto { Type = SeriesType.Book }; // only Type changes
        var afterUpdate = new Series { Id = 2, Title = "Saga", Type = SeriesType.Book, Status = SeriesStatus.Ongoing, Monitored = true };

        _mockRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(existing);
        _mockRepo.Setup(r => r.UpdateAsync(It.Is<Series>(s => s.Type == SeriesType.Book && s.Title == "Saga")))
                 .ReturnsAsync(afterUpdate);

        // Act
        var result = await _service.PatchAsync(2, patchDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Saga", result.Title);
        Assert.Equal("Book", result.Type);
        Assert.Equal("Ongoing", result.Status);
    }

    [Fact]
    public async Task PatchAsync_NonExistentId_ReturnsNull()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Series?)null);
        var patchDto = new PatchSeriesDto { Title = "Ghost" };

        // Act
        var result = await _service.PatchAsync(99, patchDto);

        // Assert
        Assert.Null(result);
        _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Series>()), Times.Never);
    }
}
