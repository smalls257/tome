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
    public async Task PatchAsync_ExistingId_AppliesChangesAndReturnsUpdatedDto()
    {
        // Arrange
        var existing = new Series
        {
            Id = 1,
            Title = "Dune",
            Type = SeriesType.Book,
            Status = SeriesStatus.Ongoing,
            Monitored = true
        };
        var patchDto = new PatchSeriesDto
        {
            Title = "Dune Messiah",
            Status = SeriesStatus.Ended
        };
        var updatedSeries = new Series
        {
            Id = 1,
            Title = "Dune Messiah",
            Type = SeriesType.Book,
            Status = SeriesStatus.Ended,
            Monitored = true
        };

        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        _mockRepo
            .Setup(r => r.UpdateAsync(It.Is<Series>(s =>
                s.Id == 1 &&
                s.Title == "Dune Messiah" &&
                s.Status == SeriesStatus.Ended)))
            .ReturnsAsync(updatedSeries);

        // Act
        var result = await _service.PatchAsync(1, patchDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Dune Messiah", result.Title);
        Assert.Equal("Ended", result.Status);
        Assert.Equal("Book", result.Type);
    }

    [Fact]
    public async Task PatchAsync_NonExistentId_ReturnsNull()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Series?)null);

        // Act
        var result = await _service.PatchAsync(99, new PatchSeriesDto { Title = "Ghost" });

        // Assert
        Assert.Null(result);
        _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Series>()), Times.Never);
    }

    [Fact]
    public async Task PatchAsync_OnlyNonNullFieldsAreApplied()
    {
        // Arrange
        var existing = new Series
        {
            Id = 2,
            Title = "Saga",
            Type = SeriesType.Comic,
            Status = SeriesStatus.Ongoing,
            Monitored = true
        };
        // Only Title is provided — Type and Status must remain unchanged
        var patchDto = new PatchSeriesDto { Title = "Saga Vol. 2" };
        var updatedSeries = new Series
        {
            Id = 2,
            Title = "Saga Vol. 2",
            Type = SeriesType.Comic,
            Status = SeriesStatus.Ongoing,
            Monitored = true
        };

        _mockRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(existing);
        _mockRepo
            .Setup(r => r.UpdateAsync(It.Is<Series>(s =>
                s.Title == "Saga Vol. 2" &&
                s.Type == SeriesType.Comic &&
                s.Status == SeriesStatus.Ongoing)))
            .ReturnsAsync(updatedSeries);

        // Act
        var result = await _service.PatchAsync(2, patchDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Saga Vol. 2", result.Title);
        Assert.Equal("Comic", result.Type);
        Assert.Equal("Ongoing", result.Status);
    }
}
