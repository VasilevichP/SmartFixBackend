using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFix.Application.Features.DeviceTypes.Commands.AddDeviceType;
using SmartFix.Application.Features.DeviceTypes.Commands.DeleteDeviceType;
using SmartFix.Application.Features.DeviceTypes.Commands.UpdateDeviceType;
using SmartFix.Application.Features.DeviceTypes.Queries.GetAllDeviceTypes;

namespace SmartFix.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class DeviceTypeController : ControllerBase
{
    private readonly ISender _mediator;

    public DeviceTypeController(ISender mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllDeviceTypesQuery());
        return Ok(result);
    }
    
    [HttpPost]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Create([FromBody] AddDeviceTypeCommand command)
    {
        await _mediator.Send(command);
        return Created();
    }
    
    [HttpPut]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Update([FromBody] UpdateDeviceTypeCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }
    
    [HttpDelete]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Delete([FromBody] DeleteDeviceTypeCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }
}