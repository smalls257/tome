using Microsoft.AspNetCore.Mvc;
using Tome.Api.Models.DTOs;
using Tome.Api.Services;

namespace Tome.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatusController(IStatusService statusService) : ControllerBase
{
    [HttpGet]
    public ActionResult<StatusDto> Get() =>
        Ok(statusService.GetStatus());
}
