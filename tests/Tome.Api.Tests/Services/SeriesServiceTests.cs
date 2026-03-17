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
    public async Task GetMonitoredAsync_WithMonitoredSeries_ReturnsMappedDtos()
    {
        // Arrange
        var monitoredSeries = new List<Series>
        {
            new() { Id = 1, Title = "Dune", Type = SeriesType.Book, Status = SeriesStatus.Ended, Monitored = true },
            new() { Id = 2, Title = "Saga", Type = SeriesType.Comic, Status = SeriesStatus.Ongoing, Monitored = true },
        };
        _mockRepo.Setup(r => r.GetMonitoredAsync()).ReturnsAsync(monitoredSeries);

        // Act
        var result = await _service.GetMonitoredAsync();

        // Assert
        var dtos = result.ToList();
        Assert.Equal(2, dtos.Count);
        Assert.All(dtos, dto => Assert.True(dto.Monitored));
        Assert.Contains(dtos, dto => dto.Title == "Dune" && dto.Type == "Book" && dto.Status == "Ended");
        Assert.Contains(dtos, dto => dto.Title == "Saga" && dto.Type == "Comic" && dto.Status == "Ongoing");
    }

    [Fact]
    public async Task GetMonitoredAsync_NoMonitoredSeries_ReturnsEmptyCollection()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetMonitoredAsync()).ReturnsAsync(new List<Series>());

        // Act
        var result = await _service.GetMonitoredAsync();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetMonitoredAsync_CallsRepositoryGetMonitoredAsync()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetMonitoredAsync()).ReturnsAsync(new List<Series>());

        // Act
        await _service.GetMonitoredAsync();

        // Assert
        _mockRepo.Verify(r => r.GetMonitoredAsync(), Times.Once);
    }
}
