using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFix.Application.Features.Requests.Commands.AssignSpecialist;
using SmartFix.Application.Features.Requests.Commands.ChangeDeviceInfo;
using SmartFix.Application.Features.Requests.Commands.ChangeRequestStatus;
using SmartFix.Application.Features.Requests.Commands.ChangeServicesList;
using SmartFix.Application.Features.Requests.Commands.CreateRequest;
using SmartFix.Application.Features.Requests.Commands.CreateRequestAsManager;
using SmartFix.Application.Features.Requests.Commands.UpdateAcceptanceInfo;
using SmartFix.Application.Features.Requests.Commands.UpdateDiagnosticsResult;
using SmartFix.Application.Features.Requests.Queries.GetAllRequestsForClient;
using SmartFix.Application.Features.Requests.Queries.GetAllRequestsForManager;
using SmartFix.Application.Features.Requests.Queries.GetRequestDetails;

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

    [HttpPost("create")]
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

    [HttpPost("createByManager")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> CreateByManager([FromBody] CreateRequestAsManagerCommand command)
    {
        var requestId = await _mediator.Send(command);
        return Ok(requestId);
    }
    
    [HttpGet("clientRequestsForClient")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> GetMyRequests()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdString, out var userId))
            return Unauthorized("Не удалось определить пользователя");

        var result = await _mediator.Send(new GetClientRequestsQuery() { ClientId = userId });

        return Ok(result);
    }

    [HttpGet("clientRequestsForManager")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> GetClientRequests([FromQuery] Guid clientId)
    {
        var result = await _mediator.Send(new GetClientRequestsQuery() { ClientId = clientId });
        return Ok(result);
    }
    
    [HttpGet("allRequestsForManager")]
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
    public async Task<IActionResult> AssignSpecialist([FromBody] AssignMasterCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("status")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> ChangeStatus([FromBody] ChangeRequestStatusCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("cancel")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Cancel([FromBody] CancelRequestCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("addService")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> AddService([FromBody] AddServiceToRequestCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("removeService")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> RemoveService([FromBody] RemoveServiceFromRequestCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("device_info")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> ChangeDeviceInfo([FromBody] ChangeDeviceInfoCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("acceptance")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> UpdateAcceptanceInfo([FromBody] UpdateAcceptanceInfoCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("diagnostics_result")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> UpdateDiagnosticsResult([FromBody] UpdateDiagnosticsResultCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
}