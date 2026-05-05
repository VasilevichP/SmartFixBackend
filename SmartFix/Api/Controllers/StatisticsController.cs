using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFix.Application.Features.Statistics.Queries.GetClientsStatistics;
using SmartFix.Application.Features.Statistics.Queries.GetMastersStatistics;
using SmartFix.Application.Features.Statistics.Queries.GetRequestsStatistics;
using SmartFix.Application.Features.Statistics.Queries.PdfReport;

namespace SmartFix.Api.Controllers;

[Route("api/[controller]")]
[Authorize(Roles = "Manager")]
public class StatisticsController:ControllerBase
{
    private readonly IMediator _mediator;

    public StatisticsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("requests")]
    public async Task<IActionResult> GetRequestsStatistics([FromQuery] GetRequestsStatisticsQuery query)
    {
        return Ok(await _mediator.Send(query));
    }

    [HttpGet("clients")]
    public async Task<IActionResult> GetClientsStatistics([FromQuery] GetClientsStatisticsQuery query)
    {
        return Ok(await _mediator.Send(query));
    }

    [HttpGet("masters")]
    public async Task<IActionResult> GetMastersStatistics([FromQuery] GetMastersStatisticsQuery query)
    {
        return Ok(await _mediator.Send(query));
    }
    [HttpGet("report")]
    public async Task<IActionResult> DownloadReportPdf([FromQuery] GetPdfReportQuery query)
    {
        var result = await _mediator.Send(query);
        return File(result.FileContents, result.ContentType, result.FileName);
    }
}