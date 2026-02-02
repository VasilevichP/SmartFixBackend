using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFix.Application.Features.DeviceModels.Queries.GetAllDeviceModels;
using SmartFix.Application.Features.Manufacturers.Commands.CreateManufacturer;
using SmartFix.Application.Features.Manufacturers.Commands.DeleteManufacturer;
using SmartFix.Application.Features.Manufacturers.Commands.UpdateManufacturer;

namespace SmartFix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeviceModelsController: ControllerBase
{
    private readonly ISender _mediator;

    public DeviceModelsController(ISender mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllDeviceModelsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpPost]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Create([FromBody] CreateDeviceModelCommand command)
    {
        await _mediator.Send(command);
        return Created();
    }
    
    [HttpPut]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Update([FromBody] UpdateDeviceModelCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }
    
    [HttpDelete]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Delete([FromBody] DeleteDeviceModelCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }
}