using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFix.Application.Features.Services.Queries.GetAllForClient;
using SmartFix.Application.Features.Services.Queries.GetAllForManager;

namespace SmartFix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicesController : ControllerBase
{
    private readonly ISender _mediator;

    public ServicesController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("client-list")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> GetClientServices()
    {
        var query = new GetAllForClientQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("manager-list")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> GetManagerServices()
    {
        var query = new GetAllForManagerQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}