using Moq;
using Tome.Api.Models;
using Tome.Api.Models.DTOs;
using Tome.Api.Repositories;
using Tome.Api.Services;

namespace Tome.Api.Tests.Services;

public class SeriesServiceTests
{
    private readonly Mock<ISeriesRepository> _mockRepository = new();
    private readonly SeriesService _service;

    public SeriesServiceTests()
    {
        _service = new SeriesService(_mockRepository.Object);
    }

    [Fact]
    public async Task GetMonitoredAsync_WithMonitoredSeries_ReturnsMappedDtos()
    {
        // Arrange
        var monitoredSeries = new List<Series>
        {
            new() { Id = 1, Title = "Dune", Type = SeriesType.Book, Status = SeriesStatus.Ended, Monitored = true },
            new() { Id = 2, Title = "Saga", Type = SeriesType.Comic, Status = SeriesStatus.Ongoing, Monitored = true },
        };
        _mockRepository.Setup(r => r.GetMonitoredAsync()).ReturnsAsync(monitoredSeries);

        // Act
        var result = await _service.GetMonitoredAsync();

        // Assert
        var list = result.ToList();
        Assert.Equal(2, list.Count);
        Assert.All(list, dto => Assert.True(dto.Monitored));
        Assert.Equal("Dune", list[0].Title);
        Assert.Equal("Book", list[0].Type);
        Assert.Equal("Ended", list[0].Status);
        Assert.Equal("Saga", list[1].Title);
        Assert.Equal("Comic", list[1].Type);
        Assert.Equal("Ongoing", list[1].Status);
    }

    [Fact]
    public async Task GetMonitoredAsync_NoMonitoredSeries_ReturnsEmptyCollection()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetMonitoredAsync()).ReturnsAsync(new List<Series>());

        // Act
        var result = await _service.GetMonitoredAsync();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetMonitoredAsync_CallsRepositoryGetMonitoredAsync()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetMonitoredAsync()).ReturnsAsync(new List<Series>());

        // Act
        await _service.GetMonitoredAsync();

        // Assert
        _mockRepository.Verify(r => r.GetMonitoredAsync(), Times.Once);
    }
}
