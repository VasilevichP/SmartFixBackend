using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFix.Application.Features.Requests.Commands.AssignPrice;
using SmartFix.Application.Features.Requests.Commands.AssignSpecialist;
using SmartFix.Application.Features.Requests.Commands.ChangeRequestStatus;
using SmartFix.Application.Features.Requests.Commands.CreateRequest;
using SmartFix.Application.Features.Requests.Queries.GetAllRequestsForClient;
using SmartFix.Application.Features.Requests.Queries.GetAllRequestsForManager;
using SmartFix.Application.Features.Requests.Queries.GetRequestDetails;
using SmartFix.Application.Features.Services.Queries.GetAllForClient;

namespace SmartFix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RequestsController : ControllerBase
{
    private readonly ISender _mediator;

    public RequestsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> Create([FromForm] CreateRequestCommand command)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            return Unauthorized("Не удалось определить пользователя");
        }
        command.ClientId = userId;
        var requestId = await _mediator.Send(command);
        return Created();
    }
    
    [HttpGet("client_list")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> GetMyRequests()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdString, out var userId))
        {
            return Unauthorized("Не удалось определить пользователя");
        }
        var result = await _mediator.Send(new GetClientRequestsQuery(){ClientId = userId});
        
        return Ok(result);
    }
    
    [HttpGet("manager-list")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> GetAllRequests([FromQuery] GetAllRequestsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetDetails(Guid id)
    {
        var query = new GetRequestDetailsQuery { RequestId = id };
        var result = await _mediator.Send(query);
        
        return Ok(result);
    }
    
    [HttpPatch("specialist")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> AssignSpecialist([FromBody] AssignSpecialistCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
    
    [HttpPatch("status")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> ChangeStatus( [FromBody] ChangeRequestStatusCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
    
    [HttpPatch("price")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> ChangePrice( [FromBody] AssignPriceCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
}