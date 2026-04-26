using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartFix.Application.Features.Discounts.Commands.ChangeDiscountStatus;
using SmartFix.Application.Features.Discounts.Commands.CreateDiscount;
using SmartFix.Application.Features.Discounts.Commands.UpdateDiscount;
using SmartFix.Application.Features.Discounts.Queries.GetAllDiscounts;
using SmartFix.Application.Features.Discounts.Queries.GetDiscountById;

namespace SmartFix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DiscountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DiscountsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("get_all")]
    public async Task<IActionResult> GetAllDiscounts()
    {
        var result = await _mediator.Send(new GetAllDiscountsQuery());
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDiscountById(Guid id)
    {
        var result = await _mediator.Send(new GetDiscountByIdQuery{Id = id});
        return Ok(result);
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateDiscount([FromBody] CreateDiscountCommand command)
    {
        await _mediator.Send(command);
        return Created();
    }
    
    [HttpPut("edit")]
    public async Task<IActionResult> UpdateDiscount([FromBody] UpdateDiscountCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
    
    [HttpPatch("change_status")]
    public async Task<IActionResult> ChangeDiscountStatus([FromBody] ChangeDiscountStatusCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
}