using Microsoft.AspNetCore.Mvc;
using Moq;
using Tome.Api.Controllers;
using Tome.Api.Models.DTOs;
using Tome.Api.Services;

namespace Tome.Api.Tests.Controllers;

public class SeriesChaptersControllerTests
{
    private readonly Mock<IChapterService> _mockService = new();
    private readonly SeriesChaptersController _controller;

    public SeriesChaptersControllerTests()
    {
        _controller = new SeriesChaptersController(_mockService.Object);
    }

    [Fact]
    public async Task GetChapters_ValidSeriesId_ReturnsOkWithChapters()
    {
        // Arrange
        var chapters = new List<ChapterDto>
        {
            new() { Id = 1, SeriesId = 42, Title = "The Awakening", Number = 1.0m },
            new() { Id = 2, SeriesId = 42, Title = "The Reckoning", Number = 2.0m },
        };
        _mockService.Setup(s => s.GetBySeriesIdAsync(42)).ReturnsAsync(chapters);

        // Act
        var result = await _controller.GetChapters(42);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var returned = Assert.IsAssignableFrom<IEnumerable<ChapterDto>>(ok.Value);
        Assert.Equal(2, returned.Count());
    }

    [Fact]
    public async Task GetChapters_ValidSeriesId_ReturnsCorrectChapterData()
    {
        // Arrange
        var chapters = new List<ChapterDto>
        {
            new() { Id = 7, SeriesId = 5, Title = "Origin", Number = 0.5m },
        };
        _mockService.Setup(s => s.GetBySeriesIdAsync(5)).ReturnsAsync(chapters);

        // Act
        var result = await _controller.GetChapters(5);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var returned = Assert.IsAssignableFrom<IEnumerable<ChapterDto>>(ok.Value);
        var chapter = returned.Single();
        Assert.Equal(7, chapter.Id);
        Assert.Equal(5, chapter.SeriesId);
        Assert.Equal("Origin", chapter.Title);
        Assert.Equal(0.5m, chapter.Number);
    }

    [Fact]
    public async Task GetChapters_SeriesHasNoChapters_ReturnsOkWithEmptyList()
    {
        // Arrange
        _mockService.Setup(s => s.GetBySeriesIdAsync(10)).ReturnsAsync(new List<ChapterDto>());

        // Act
        var result = await _controller.GetChapters(10);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var returned = Assert.IsAssignableFrom<IEnumerable<ChapterDto>>(ok.Value);
        Assert.Empty(returned);
    }

    [Fact]
    public async Task GetChapters_SeriesNotFound_ReturnsNotFound()
    {
        // Arrange
        _mockService.Setup(s => s.GetBySeriesIdAsync(99)).ReturnsAsync((IEnumerable<ChapterDto>?)null);

        // Act
        var result = await _controller.GetChapters(99);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
}
