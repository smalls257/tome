using Microsoft.AspNetCore.Mvc;
using Moq;
using Tome.Api.Controllers;
using Tome.Api.Models.DTOs;
using Tome.Api.Services;

namespace Tome.Api.Tests.Controllers;

public class SeriesControllerTests
{
    private readonly Mock<ISeriesService> _mockService = new();
    private readonly SeriesController _controller;

    public SeriesControllerTests()
    {
        _controller = new SeriesController(_mockService.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsSeries()
    {
        // Arrange
        var series = new List<SeriesDto>
        {
            new() { Id = 1, Title = "Dune", Type = "Book", Status = "Ended", Monitored = true },
            new() { Id = 2, Title = "Saga", Type = "Comic", Status = "Ongoing", Monitored = true },
        };
        _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(series);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var returned = Assert.IsAssignableFrom<IEnumerable<SeriesDto>>(ok.Value);
        Assert.Equal(2, returned.Count());
    }

    [Fact]
    public async Task GetById_ValidId_ReturnsSeries()
    {
        // Arrange
        var series = new SeriesDto { Id = 1, Title = "Dune", Type = "Book", Status = "Ended", Monitored = true };
        _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(series);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var returned = Assert.IsType<SeriesDto>(ok.Value);
        Assert.Equal("Dune", returned.Title);
    }

    [Fact]
    public async Task GetById_InvalidId_ReturnsNotFound()
    {
        // Arrange
        _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((SeriesDto?)null);

        // Act
        var result = await _controller.GetById(99);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Create_ValidDto_ReturnsCreated()
    {
        // Arrange
        var dto = new CreateSeriesDto { Title = "The Expanse", Type = Models.SeriesType.Book };
        var created = new SeriesDto { Id = 3, Title = "The Expanse", Type = "Book", Status = "Ongoing", Monitored = true };
        _mockService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(created);

        // Act
        var result = await _controller.Create(dto);

        // Assert
        var createdAt = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returned = Assert.IsType<SeriesDto>(createdAt.Value);
        Assert.Equal("The Expanse", returned.Title);
        Assert.Equal(3, returned.Id);
    }

    [Fact]
    public async Task GetAll_WithStatusFilter_ReturnsMatchingSeries()
    {
        // Arrange
        var ongoing = new List<SeriesDto>
        {
            new() { Id = 1, Title = "Saga", Type = "Comic", Status = "Ongoing", Monitored = true },
            new() { Id = 2, Title = "The Expanse", Type = "Book", Status = "Ongoing", Monitored = false },
        };
        _mockService
            .Setup(s => s.GetByStatusAsync(Models.SeriesStatus.Ongoing))
            .ReturnsAsync(ongoing);

        // Act
        var result = await _controller.GetAll(Models.SeriesStatus.Ongoing);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var returned = Assert.IsAssignableFrom<IEnumerable<SeriesDto>>(ok.Value);
        Assert.Equal(2, returned.Count());
        Assert.All(returned, s => Assert.Equal("Ongoing", s.Status));
    }

    [Fact]
    public async Task GetAll_WithStatusFilter_NoMatches_ReturnsEmptyArray()
    {
        // Arrange
        _mockService
            .Setup(s => s.GetByStatusAsync(Models.SeriesStatus.Cancelled))
            .ReturnsAsync(new List<SeriesDto>());

        // Act
        var result = await _controller.GetAll(Models.SeriesStatus.Cancelled);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var returned = Assert.IsAssignableFrom<IEnumerable<SeriesDto>>(ok.Value);
        Assert.Empty(returned);
    }

    [Fact]
    public async Task GetAll_WithoutStatusFilter_CallsGetAllAsync()
    {
        // Arrange
        var allSeries = new List<SeriesDto>
        {
            new() { Id = 1, Title = "Dune", Type = "Book", Status = "Ended", Monitored = true },
            new() { Id = 2, Title = "Saga", Type = "Comic", Status = "Ongoing", Monitored = true },
        };
        _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(allSeries);

        // Act
        var result = await _controller.GetAll(null);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var returned = Assert.IsAssignableFrom<IEnumerable<SeriesDto>>(ok.Value);
        Assert.Equal(2, returned.Count());
        _mockService.Verify(s => s.GetAllAsync(), Times.Once);
        _mockService.Verify(s => s.GetByStatusAsync(It.IsAny<Models.SeriesStatus>()), Times.Never);
    }
}
