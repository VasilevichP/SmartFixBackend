using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFix.Application.Features.Clients.Commands.CreateProfileByManager;
using SmartFix.Application.Features.Clients.Commands.UpdateProfileByManager;
using SmartFix.Application.Features.Clients.Queries.GetAllClients;
using SmartFix.Application.Features.Clients.Queries.GetProfile;
using SmartFix.Application.Features.Users.Commands.UpdateProfileByClient;

namespace SmartFix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClientsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClientsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetClientProfile(Guid id)
    {
        var profile = await _mediator.Send(new GetClientProfileQuery { UserId = id });
        return Ok(profile);
    }

    [HttpPut]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> UpdateProfileByClient([FromBody] UpdateProfileByClientCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }
    
    [HttpGet("getAll")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> GetClientList([FromQuery] GetAllClientsQuery query)
    {
        var profile = await _mediator.Send(query);
        return Ok(profile);
    }
    
    [HttpPost("create")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> CreateProfileByManager([FromBody] CreateProfileByManagerCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }
    
    [HttpPut("update")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> UpdateProfileByManager([FromBody] UpdateProfileByManagerCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }
}