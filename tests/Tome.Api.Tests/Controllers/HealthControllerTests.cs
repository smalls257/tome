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
    public async Task Get_ServiceReturnsHealthDto_Returns200Ok()
    {
        // Arrange
        var dto = new HealthDto { Status = "Healthy", Version = "1.0.0.0" };
        _mockService.Setup(s => s.GetHealthAsync()).ReturnsAsync(dto);

        // Act
        var result = await _controller.Get();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var returned = Assert.IsType<HealthDto>(ok.Value);
        Assert.Equal("Healthy", returned.Status);
        Assert.Equal("1.0.0.0", returned.Version);
    }

    [Fact]
    public async Task Get_ServiceReturnsDegradedStatus_Returns200OkWithDegradedStatus()
    {
        // Arrange
        var dto = new HealthDto { Status = "Degraded", Version = "2.3.1.0" };
        _mockService.Setup(s => s.GetHealthAsync()).ReturnsAsync(dto);

        // Act
        var result = await _controller.Get();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var returned = Assert.IsType<HealthDto>(ok.Value);
        Assert.Equal("Degraded", returned.Status);
        Assert.Equal("2.3.1.0", returned.Version);
    }
}
