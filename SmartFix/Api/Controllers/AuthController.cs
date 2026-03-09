using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartFix.Application.Features.Users.Commands.AuthorizeUser;
using SmartFix.Application.Features.Users.Commands.RegisterClient;

namespace SmartFix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(RegisterUserCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    [HttpPost("authorize")]
    public async Task<IActionResult> AuthorizeUser(AuthorizeUserCommand command)
    {
        var token = await _mediator.Send(command);
        return Ok(new { Token = token });
    }
}