using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartFix.Application.Features.Masters.Commands.CreateMaster;
using SmartFix.Application.Features.Masters.Commands.DeleteMaster;
using SmartFix.Application.Features.Masters.Commands.UpdateMaster;
using SmartFix.Application.Features.Masters.Queries.GetAllMasters;
using SmartFix.Application.Features.Masters.Queries.GetMasterById;
using SmartFix.Domain.Aggregates;

namespace SmartFix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MastersController : ControllerBase
{
    private readonly IMediator _mediator;

    public MastersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("get_all")]
    public async Task<IActionResult> GetAllMasters([FromQuery] GetAllMastersQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMasterById(Guid id)
    {
        var result = await _mediator.Send(new GetMasterByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateMaster([FromBody] CreateMasterCommand command)
    {
        await _mediator.Send(command);
        return Created();
    }

    [HttpPut("edit")]
    public async Task<IActionResult> UpdateMaster([FromBody] UpdateMasterCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("delete")]
    public async Task<IActionResult> DeleteMaster([FromBody] DeleteMasterCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
}