using Microsoft.AspNetCore.Mvc;
using Tome.Api.Models.DTOs;
using Tome.Api.Services;

namespace Tome.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController(IHealthService healthService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<HealthDto>> Get() =>
        Ok(await healthService.GetAsync());
}
