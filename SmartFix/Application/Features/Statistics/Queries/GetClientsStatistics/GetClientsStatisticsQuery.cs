using MediatR;
using SmartFix.Application.Common.Interfaces;
using SmartFix.Application.Features.Statistics.DTO;

namespace SmartFix.Application.Features.Statistics.Queries.GetClientsStatistics;

public class GetClientsStatisticsQuery:IRequest<ClientsStatsDto>{
    public string Period { get; set; } = "month";
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}