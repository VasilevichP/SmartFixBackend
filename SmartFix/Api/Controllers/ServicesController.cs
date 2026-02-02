using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFix.Application.Features.Services.Commands.ChangeVisibility;
using SmartFix.Application.Features.Services.Commands.CreateService;
using SmartFix.Application.Features.Services.Commands.DeleteService;
using SmartFix.Application.Features.Services.Commands.UpdateService;
using SmartFix.Application.Features.Services.Queries;
using SmartFix.Application.Features.Services.Queries.GetAllForClient;
using SmartFix.Application.Features.Services.Queries.GetAllForManager;

namespace SmartFix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicesController : ControllerBase
{
    private readonly ISender _mediator;

    public ServicesController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("client-list")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> GetClientServices([FromQuery] GetAllServicesForManagerQuery filterParams)
    {
        var result = await _mediator.Send(filterParams);
        return Ok(result);
    }

    [HttpGet("manager-list")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> GetManagerServices([FromQuery] GetAllServicesForManagerQuery filterParams)
    {
        var result = await _mediator.Send(filterParams);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDetails(Guid id)
    {
        var result = await _mediator.Send(new GetServiceDetailsQuery { ServiceId = id });
        return Ok(result);
    }
    
    [HttpPost]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> CreateService([FromBody] CreateServiceCommand command)
    {
        await _mediator.Send(command);
        return Created();
    }

    [HttpPut]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> UpdateService([FromBody] UpdateServiceCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    [HttpPatch]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> ToggleVisibility([FromBody] ToggleServiceVisibilityCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> DeleteService([FromBody] DeleteServiceCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
}