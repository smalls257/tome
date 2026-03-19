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
    public async Task UpdateAsync_ValidId_ReturnsMappedDto()
    {
        // Arrange
        var dto = new UpdateSeriesDto
        {
            Title = "Saga Vol. 2",
            Type = SeriesType.Comic,
            Status = SeriesStatus.Ongoing,
            Monitored = true,
        };
        var updatedDomain = new Series
        {
            Id = 2,
            Title = "Saga Vol. 2",
            Type = SeriesType.Comic,
            Status = SeriesStatus.Ongoing,
            Monitored = true,
        };
        _mockRepo
            .Setup(r => r.UpdateAsync(2, It.Is<Series>(s =>
                s.Title == dto.Title &&
                s.Type == dto.Type &&
                s.Status == dto.Status &&
                s.Monitored == dto.Monitored)))
            .ReturnsAsync(updatedDomain);

        // Act
        var result = await _service.UpdateAsync(2, dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Id);
        Assert.Equal("Saga Vol. 2", result.Title);
        Assert.Equal("Comic", result.Type);
        Assert.Equal("Ongoing", result.Status);
        Assert.True(result.Monitored);
    }

    [Fact]
    public async Task UpdateAsync_InvalidId_ReturnsNull()
    {
        // Arrange
        var dto = new UpdateSeriesDto
        {
            Title = "Nonexistent",
            Type = SeriesType.Book,
            Status = SeriesStatus.Cancelled,
            Monitored = false,
        };
        _mockRepo
            .Setup(r => r.UpdateAsync(99, It.IsAny<Series>()))
            .ReturnsAsync((Series?)null);

        // Act
        var result = await _service.UpdateAsync(99, dto);

        // Assert
        Assert.Null(result);
    }
}
