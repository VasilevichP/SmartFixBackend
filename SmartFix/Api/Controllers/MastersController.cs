using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFix.Application.Features.Masters.Commands.CreateMaster;
using SmartFix.Application.Features.Masters.Commands.DeleteMaster;
using SmartFix.Application.Features.Masters.Commands.UpdateMaster;
using SmartFix.Application.Features.Masters.Queries.GetAllMasters;
using SmartFix.Application.Features.Masters.Queries.GetAllMastersForSelect;
using SmartFix.Application.Features.Masters.Queries.GetMasterById;
using SmartFix.Domain.Aggregates;

namespace SmartFix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MastersController : ControllerBase
{
    private readonly IMediator _mediator;

    public MastersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("getAll")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> GetAllMasters([FromQuery] GetAllMastersQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet("getAllForSelect")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> GetAllMastersForSelect()
    {
        var result = await _mediator.Send(new GetAllMastersForSelectQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> GetMasterById(Guid id)
    {
        var result = await _mediator.Send(new GetMasterByIdQuery { Id = id });
        return Ok(result);
    }
    
    [HttpGet("profile")]
    [Authorize(Roles = "Master")]
    public async Task<IActionResult> GetMasterProfile()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            return Unauthorized("Не удалось определить пользователя");
        }
        var result = await _mediator.Send(new GetMasterByIdQuery { Id = userId });
        return Ok(result);
    }

    [HttpPost("create")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> CreateMaster([FromBody] CreateMasterCommand command)
    {
        await _mediator.Send(command);
        return Created();
    }

    [HttpPut("edit")]
    [Authorize(Roles = "Manager, Master")]
    public async Task<IActionResult> UpdateMaster([FromBody] UpdateMasterCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("delete")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> DeleteMaster([FromBody] DeleteMasterCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
}