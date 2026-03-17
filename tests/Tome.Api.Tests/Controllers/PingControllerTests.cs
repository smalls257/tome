using Microsoft.AspNetCore.Mvc;
using Tome.Api.Controllers;

namespace Tome.Api.Tests.Controllers;

public class PingControllerTests
{
    private readonly PingController _controller = new();

    [Fact]
    public void Get_Always_ReturnsOkWithPongBody()
    {
        // Arrange — no setup required; PingController has no dependencies

        // Act
        var result = _controller.Get();

        // Assert
        var content = Assert.IsType<ContentResult>(result.Result);
        Assert.Equal("pong", content.Content);
    }

    [Fact]
    public void Get_Always_ReturnsTextPlainContentType()
    {
        // Arrange — no setup required; PingController has no dependencies

        // Act
        var result = _controller.Get();

        // Assert
        var content = Assert.IsType<ContentResult>(result.Result);
        Assert.Equal("text/plain", content.ContentType);
    }
}
