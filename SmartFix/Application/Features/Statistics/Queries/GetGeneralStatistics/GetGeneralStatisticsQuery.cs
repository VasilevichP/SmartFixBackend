using MediatR;
using SmartFix.Application.Common.Interfaces;
using SmartFix.Application.Features.Statistics.DTO;

namespace SmartFix.Application.Features.Statistics.Queries.GetGeneralStatistics;

public class GetGeneralStatisticsQuery:IRequest<GeneralStatsDto>,IDateRangeRequest
{
    public string Period { get; set; } = "month"; 
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}