using Microsoft.AspNetCore.Mvc;

namespace Tome.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PingController : ControllerBase
{
    [HttpGet]
    public ActionResult<string> Get() =>
        Content("pong", "text/plain");
}
