using Microsoft.AspNetCore.Mvc;
using Moq;
using Tome.Api.Controllers;
using Tome.Api.Models.DTOs;
using Tome.Api.Services;

namespace Tome.Api.Tests.Controllers;

public class HealthControllerTests
{
    private readonly Mock<IHealthService> _mockService = new();
    private readonly HealthController _controller;

    public HealthControllerTests()
    {
        _controller = new HealthController(_mockService.Object);
    }

    [Fact]
    public async Task Get_ServiceReturnsHealthDto_ReturnsOkResult()
    {
        // Arrange
        var dto = new HealthDto { Status = "ok", Timestamp = DateTimeOffset.UtcNow };
        _mockService.Setup(s => s.GetAsync()).ReturnsAsync(dto);

        // Act
        var result = await _controller.Get();

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task Get_ServiceReturnsHealthDto_ResponseContainsExpectedStatusAndTimestamp()
    {
        // Arrange
        var expectedTimestamp = new DateTimeOffset(2026, 3, 16, 20, 51, 0, TimeSpan.Zero);
        var dto = new HealthDto { Status = "ok", Timestamp = expectedTimestamp };
        _mockService.Setup(s => s.GetAsync()).ReturnsAsync(dto);

        // Act
        var result = await _controller.Get();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var returned = Assert.IsType<HealthDto>(ok.Value);
        Assert.Equal("ok", returned.Status);
        Assert.Equal(expectedTimestamp, returned.Timestamp);
    }
}
