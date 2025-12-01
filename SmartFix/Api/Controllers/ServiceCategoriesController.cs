using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFix.Application.Features.ServiceCategories.Commands.CreateCategory;
using SmartFix.Application.Features.ServiceCategories.Commands.DeleteCategory;
using SmartFix.Application.Features.ServiceCategories.Commands.UpdateCategory;
using SmartFix.Application.Features.ServiceCategories.Queries.GetAll;

namespace SmartFix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiceCategoriesController : ControllerBase
{
    private readonly ISender _mediator;

    public ServiceCategoriesController(ISender mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllCategoriesQuery());
        return Ok(result);
    }
    
    [HttpPost]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
    {
        await _mediator.Send(command);
        return Created();
    }
    
    [HttpPut]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Update([FromBody] UpdateCategoryCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }
    
    [HttpDelete]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Delete([FromBody] DeleteCategoryCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }
}