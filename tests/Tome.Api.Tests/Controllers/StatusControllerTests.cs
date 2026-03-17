using Microsoft.AspNetCore.Mvc;
using Moq;
using Tome.Api.Controllers;
using Tome.Api.Models.DTOs;
using Tome.Api.Services;

namespace Tome.Api.Tests.Controllers;

public class StatusControllerTests
{
    private readonly Mock<IStatusService> _mockService = new();
    private readonly StatusController _controller;

    public StatusControllerTests()
    {
        _controller = new StatusController(_mockService.Object);
    }

    [Fact]
    public void Get_WhenCalled_ReturnsOk()
    {
        // Arrange
        var dto = new StatusDto { UptimeSeconds = 42 };
        _mockService.Setup(s => s.GetStatus()).Returns(dto);

        // Act
        var result = _controller.Get();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, ok.StatusCode);
    }

    [Fact]
    public void Get_WhenCalled_ReturnsStatusDtoWithUptimeSeconds()
    {
        // Arrange
        var dto = new StatusDto { UptimeSeconds = 123 };
        _mockService.Setup(s => s.GetStatus()).Returns(dto);

        // Act
        var result = _controller.Get();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var returned = Assert.IsType<StatusDto>(ok.Value);
        Assert.Equal(123, returned.UptimeSeconds);
    }

    [Fact]
    public void Get_UptimeSeconds_IsNonNegative()
    {
        // Arrange
        var dto = new StatusDto { UptimeSeconds = 0 };
        _mockService.Setup(s => s.GetStatus()).Returns(dto);

        // Act
        var result = _controller.Get();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var returned = Assert.IsType<StatusDto>(ok.Value);
        Assert.True(returned.UptimeSeconds >= 0);
    }
}
