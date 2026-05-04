using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFix.Application.Features.PromoCodes.Commands.ChangePromoCodeStatus;
using SmartFix.Application.Features.PromoCodes.Commands.CreatePromoCode;
using SmartFix.Application.Features.PromoCodes.Commands.UpdatePromoCode;
using SmartFix.Application.Features.PromoCodes.Queries.GetAllPromoCodes;
using SmartFix.Application.Features.PromoCodes.Queries.GetPromoCodeById;

namespace SmartFix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PromoCodesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PromoCodesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("getAll")]
    public async Task<IActionResult> GetAllPromoCodes()
    {
        var result = await _mediator.Send(new GetAllPromoCodesQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPromoCodeById(Guid id)
    {
        var result = await _mediator.Send(new GetPromoCodeByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreatePromoCode([FromBody] CreatePromoCodeCommand command)
    {
        await _mediator.Send(command);
        return Created();
    }

    [HttpPut("edit")]
    public async Task<IActionResult> UpdatePromoCode([FromBody] UpdatePromoCodeCommand command)
    {
        await _mediator.Send(command);
        return Created();
    }

    [HttpPatch("changeStatus")]
    public async Task<IActionResult> ChangePromoCodeStatus([FromBody] ChangePromoCodeStatusCommand command)
    {
        await _mediator.Send(command);
        return Created();
    }
}