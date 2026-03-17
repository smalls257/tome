using Moq;
using Tome.Api.Models;
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
    public async Task GetMonitoredAsync_WithMonitoredSeries_ReturnsMappedDtos()
    {
        // Arrange
        var series = new List<Series>
        {
            new() { Id = 1, Title = "Dune", Type = SeriesType.Book, Status = SeriesStatus.Ended, Monitored = true },
            new() { Id = 2, Title = "Saga", Type = SeriesType.Comic, Status = SeriesStatus.Ongoing, Monitored = true },
        };
        _mockRepo.Setup(r => r.GetMonitoredAsync()).ReturnsAsync(series);

        // Act
        var result = await _service.GetMonitoredAsync();

        // Assert
        var dtos = result.ToList();
        Assert.Equal(2, dtos.Count);
        Assert.Equal("Dune", dtos[0].Title);
        Assert.Equal("Book", dtos[0].Type);
        Assert.Equal("Ended", dtos[0].Status);
        Assert.True(dtos[0].Monitored);
        Assert.Equal("Saga", dtos[1].Title);
        Assert.True(dtos[1].Monitored);
    }

    [Fact]
    public async Task GetMonitoredAsync_WithNoMonitoredSeries_ReturnsEmptyCollection()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetMonitoredAsync()).ReturnsAsync(new List<Series>());

        // Act
        var result = await _service.GetMonitoredAsync();

        // Assert
        Assert.Empty(result);
    }
}
