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
    public async Task GetByStatusAsync_WithMatchingStatus_ReturnsMappedDtos()
    {
        // Arrange
        var ongoing = new List<Series>
        {
            new() { Id = 1, Title = "Saga", Type = SeriesType.Comic, Status = SeriesStatus.Ongoing, Monitored = true },
            new() { Id = 2, Title = "The Expanse", Type = SeriesType.Book, Status = SeriesStatus.Ongoing, Monitored = false },
        };
        _mockRepo
            .Setup(r => r.GetByStatusAsync(SeriesStatus.Ongoing))
            .ReturnsAsync(ongoing);

        // Act
        var result = (await _service.GetByStatusAsync(SeriesStatus.Ongoing)).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, dto => Assert.Equal("Ongoing", dto.Status));
        Assert.Equal("Saga", result[0].Title);
        Assert.Equal("Comic", result[0].Type);
        Assert.Equal("The Expanse", result[1].Title);
        Assert.Equal("Book", result[1].Type);
    }

    [Fact]
    public async Task GetByStatusAsync_NoMatchingStatus_ReturnsEmptyCollection()
    {
        // Arrange
        _mockRepo
            .Setup(r => r.GetByStatusAsync(SeriesStatus.Cancelled))
            .ReturnsAsync(new List<Series>());

        // Act
        var result = await _service.GetByStatusAsync(SeriesStatus.Cancelled);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByStatusAsync_MapsAllFieldsCorrectly()
    {
        // Arrange
        var series = new List<Series>
        {
            new()
            {
                Id = 42,
                Title = "Watchmen",
                Type = SeriesType.Comic,
                Status = SeriesStatus.Ended,
                Monitored = true,
            },
        };
        _mockRepo
            .Setup(r => r.GetByStatusAsync(SeriesStatus.Ended))
            .ReturnsAsync(series);

        // Act
        var result = (await _service.GetByStatusAsync(SeriesStatus.Ended)).ToList();

        // Assert
        var dto = Assert.Single(result);
        Assert.Equal(42, dto.Id);
        Assert.Equal("Watchmen", dto.Title);
        Assert.Equal("Comic", dto.Type);
        Assert.Equal("Ended", dto.Status);
        Assert.True(dto.Monitored);
    }
}
