using Moq;
using Tome.Api.Models;
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
    public async Task DeleteAsync_ExistingId_ReturnsTrue()
    {
        // Arrange
        _mockRepository.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        Assert.True(result);
        _mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_NonExistingId_ReturnsFalse()
    {
        // Arrange
        _mockRepository.Setup(r => r.DeleteAsync(99)).ReturnsAsync(false);

        // Act
        var result = await _service.DeleteAsync(99);

        // Assert
        Assert.False(result);
        _mockRepository.Verify(r => r.DeleteAsync(99), Times.Once);
    }
}
