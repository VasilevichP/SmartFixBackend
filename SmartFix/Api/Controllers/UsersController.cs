using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFix.Application.Features.Users.Commands.UpdateProfile;
using SmartFix.Application.Features.Users.Queries.GetProfile;

namespace SmartFix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController: ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetProfile()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var profile = await _mediator.Send(new GetUserProfileQuery{UserId = Guid.Parse(userId)});
        return Ok(profile);
    }
    [HttpPut]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }
}