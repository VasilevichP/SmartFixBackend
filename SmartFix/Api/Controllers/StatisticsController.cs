using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFix.Application.Features.Statistics.Queries.GetClientsStatistics;
using SmartFix.Application.Features.Statistics.Queries.GetGeneralStatistics;
using SmartFix.Application.Features.Statistics.Queries.GetServicesStatistics;
using SmartFix.Application.Features.Statistics.Queries.GetSpecialistsStatistics;

namespace SmartFix.Api.Controllers;

[Route("api/[controller]")]
public class StatisticsController:ControllerBase
{
    private readonly IMediator _mediator;

    public StatisticsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("general")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> GetGeneralStatistics([FromQuery] GetGeneralStatisticsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    [HttpGet("services")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> GetServicesStatistics([FromQuery] GetServicesStatisticsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    [HttpGet("clients")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> GetClientsStatistics([FromQuery] GetClientsStatisticsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    [HttpGet("specialists")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> GetSpecialistsStatistics([FromQuery] GetSpecialistsStatisticsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}