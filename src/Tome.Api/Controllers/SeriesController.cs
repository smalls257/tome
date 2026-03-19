using Microsoft.AspNetCore.Mvc;
using Tome.Api.Models.DTOs;
using Tome.Api.Services;

namespace Tome.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeriesController(ISeriesService seriesService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SeriesDto>>> GetAll() =>
        Ok(await seriesService.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SeriesDto>> GetById(int id)
    {
        var series = await seriesService.GetByIdAsync(id);
        return series is null ? NotFound() : Ok(series);
    }

    [HttpPost]
    public async Task<ActionResult<SeriesDto>> Create(CreateSeriesDto dto)
    {
        var created = await seriesService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<SeriesDto>> Update(int id, UpdateSeriesDto dto)
    {
        var updated = await seriesService.UpdateAsync(id, dto);
        return updated is null ? NotFound() : Ok(updated);
    }
}
