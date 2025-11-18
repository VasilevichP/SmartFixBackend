using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFix.Application.Features.ServiceCategories.Queries.GetAll;
using SmartFix.Application.Features.Specialists.Commands.AddSpecialist;
using SmartFix.Application.Features.Specialists.Commands.DeleteSpecialist;
using SmartFix.Application.Features.Specialists.Commands.UpdateSpecialist;

namespace SmartFix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Manager")]
public class SpecialistsController : ControllerBase
{
    private readonly ISender _mediator;

    public SpecialistsController(ISender mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllQuery());
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddSpecialistCommand command)
    {
        await _mediator.Send(command);
        return Created();
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateSpecialistCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] UpdateSpecialistCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
}