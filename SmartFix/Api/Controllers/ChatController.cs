using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFix.Application.Features.Chats.Commands.SendMessage;
using SmartFix.Application.Features.Chats.Queries.GetChatHistory;

namespace SmartFix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController:ControllerBase
{
    private readonly IMediator _mediator;

    public ChatController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("{clientId}")]
    [Authorize(Roles = "Manager, Client")]
    public async Task<IActionResult> GetClientChatHistory(Guid clientId)
    {
        var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        var userIdString = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (role == "Client" && userIdString != clientId.ToString())
        {
            return Forbid("Вы не можете просматривать чужую переписку");
        }

        var result = await _mediator.Send(new GetChatHistoryQuery { ClientId = clientId });
        return Ok(result);
    }
    
    [HttpPost("send")]
    [Authorize(Roles = "Manager, Client")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageCommand command)
    {
        var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        var userIdString = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        
        if (Guid.TryParse(userIdString, out Guid userId))
        {
            if (role == "Client")
            {
                command.ClientId = userId;
                command.IsFromClient = true;
            }
            else if (role == "Manager")
            {
                command.IsFromClient = false;
            }
            else
            {
                return Forbid("У вас нет доступа к чату");
            }
        }

        var result = await _mediator.Send(command);
        
        return Ok(result);
    }
}