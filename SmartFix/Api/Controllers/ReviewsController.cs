using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFix.Application.Features.Reviews.Commands.CreateReview;
using SmartFix.Application.Features.Reviews.Commands.DeleteReview;

namespace SmartFix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly ISender _mediator;

    public ReviewsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> Create([FromBody] CreateReviewCommand command)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdString, out var userId)) return Unauthorized();

        command.ClientId = userId;
        await _mediator.Send(command);
        return Ok();
    }

    [HttpDelete]
    [Authorize] 
    public async Task<IActionResult> Delete([FromBody] DeleteReviewCommand command)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

        if (!Guid.TryParse(userIdString, out var userId)) return Unauthorized();

        command.UserId = userId;
        command.UserRole = userRole;

        await _mediator.Send(command);
        return NoContent();
    }
}