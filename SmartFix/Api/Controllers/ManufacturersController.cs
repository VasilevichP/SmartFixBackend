using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFix.Application.Features.Manufacturers.Commands.CreateManufacturer;
using SmartFix.Application.Features.Manufacturers.Commands.DeleteManufacturer;
using SmartFix.Application.Features.Manufacturers.Commands.UpdateManufacturer;
using SmartFix.Application.Features.Manufacturers.Queries.GetAllManufacturers;

namespace SmartFix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ManufacturersController: ControllerBase
{
    private readonly ISender _mediator;

    public ManufacturersController(ISender mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllManufacturersQuery());
        return Ok(result);
    }
    
    [HttpPost]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Create([FromBody] CreateManufacturerCommand command)
    {
        await _mediator.Send(command);
        return Created();
    }
    
    [HttpPut]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Update([FromBody] UpdateManufacturerCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }
    
    [HttpDelete]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Delete([FromBody] DeleteManufacturerCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }
}