using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFix.Application.Features.Documents.Queries.GetRequestDocuments;
using SmartFix.Application.Features.Requests.Commands.ApproveRequest;
using SmartFix.Application.Features.Requests.Commands.AssignMaster;
using SmartFix.Application.Features.Requests.Commands.ChangeContactInfo;
using SmartFix.Application.Features.Requests.Commands.ChangeDeviceInfo;
using SmartFix.Application.Features.Requests.Commands.ChangeFieldRequestInfo;
using SmartFix.Application.Features.Requests.Commands.ChangeRequestStatus;
using SmartFix.Application.Features.Requests.Commands.CreateRequest;
using SmartFix.Application.Features.Requests.Commands.CreateRequestAsManager;
using SmartFix.Application.Features.Requests.Commands.LeaveReview;
using SmartFix.Application.Features.Requests.Commands.RejectRequest;
using SmartFix.Application.Features.Requests.Commands.UpdateAcceptanceInfo;
using SmartFix.Application.Features.Requests.Commands.UpdateDiagnosticsResult;
using SmartFix.Application.Features.Requests.Commands.UpdateRequestServicesList;
using SmartFix.Application.Features.Requests.Queries.GetAllRequestsForClient;
using SmartFix.Application.Features.Requests.Queries.GetAllRequestsForManager;
using SmartFix.Application.Features.Requests.Queries.GetAllRequestsForMaster;
using SmartFix.Application.Features.Requests.Queries.GetClosedRequestsForClient;
using SmartFix.Application.Features.Requests.Queries.GetRequestDetails;
using SmartFix.Domain.ValueObjects;

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

    [HttpGet("masterRequestsForMaster")]
    [Authorize(Roles = "Master")]
    public async Task<IActionResult> GetMasterRequests([FromQuery] GetAllRequestsForMasterQuery query)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdString, out var userId))
            return Unauthorized("Не удалось определить пользователя");
        query.MasterId = userId;

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("clientRequestsForManager")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> GetClientRequests([FromQuery] Guid clientId)
    {
        var result = await _mediator.Send(new GetClientRequestsQuery() { ClientId = clientId });
        return Ok(result);
    }

    [HttpGet("closedClientRequests")]
    [Authorize(Roles = "Manager,Client")]
    public async Task<IActionResult> GetClosedClientRequests([FromQuery] Guid clientId)
    {
        var result = await _mediator.Send(new GetClosedRequestsForClientQuery() { ClientId = clientId });
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
        var result = await _mediator.Send(new GetRequestDetailsQuery { RequestId = id });
        return Ok(result);
    }

    [HttpPatch("master")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> AssignMaster([FromBody] AssignMasterCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("status")]
    [Authorize(Roles = "Manager,Master")]
    public async Task<IActionResult> ChangeStatus([FromBody] ChangeRequestStatusCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("cancel")]
    [Authorize(Roles = "Manager,Master")]
    public async Task<IActionResult> Cancel([FromBody] CancelRequestCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("services")]
    [Authorize(Roles = "Manager,Master")]
    public async Task<IActionResult> UpdateServices([FromBody] UpdateRequestServicesListCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("contactInfo")]
    [Authorize(Roles = "Manager,Master")]
    public async Task<IActionResult> ChangeContactInfo([FromBody] ChangeContactInfoCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("fieldRequestInfo")]
    [Authorize(Roles = "Manager,Master")]
    public async Task<IActionResult> ChangeFieldRequestInfo([FromBody] ChangeFieldRequestInfoCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("deviceInfo")]
    [Authorize(Roles = "Manager,Master")]
    public async Task<IActionResult> ChangeDeviceInfo([FromBody] ChangeDeviceInfoCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("acceptance")]
    [Authorize(Roles = "Manager,Master")]
    public async Task<IActionResult> UpdateAcceptanceInfo([FromBody] UpdateAcceptanceInfoCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("diagnosticsResult")]
    [Authorize(Roles = "Manager,Master")]
    public async Task<IActionResult> UpdateDiagnosticsResult([FromBody] UpdateDiagnosticsResultCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("leaveReview")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> LeaveReview([FromBody] LeaveReviewCommand command)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            return Unauthorized("Не удалось определить пользователя");
        }

        command.ClientId = userId;
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("approve")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> ApproveRequest([FromBody] ApproveRequestCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("reject")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> RejectRequest([FromBody] RejectRequestCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpGet("{id}/acceptanceAct")]
    [Authorize]
    public async Task<IActionResult> DownloadAcceptanceAct(Guid id)
    {
        var result = await _mediator.Send(new GetRequestDocumentsQuery()
            { RequestId = id, Type = DocumentType.Acceptance });
        return File(result.FileContents, result.ContentType, result.FileName);
    }

    [HttpGet("{requestId}/completionAct")]
    [Authorize]
    public async Task<IActionResult> DownloadCompletionAct(Guid requestId)
    {
        var result = await _mediator.Send(new GetRequestDocumentsQuery
            { RequestId = requestId, Type = DocumentType.Completion });
        return File(result.FileContents, result.ContentType, result.FileName);
    }

    [HttpGet("{requestId}/warrantyCard")]
    [Authorize]
    public async Task<IActionResult> DownloadWarrantyCard(Guid requestId)
    {
        var result = await _mediator.Send(new GetRequestDocumentsQuery
            { RequestId = requestId, Type = DocumentType.Warranty });
        return File(result.FileContents, result.ContentType, result.FileName);
    }
}