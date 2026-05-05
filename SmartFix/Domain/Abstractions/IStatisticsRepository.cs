using SmartFix.Application.Features.Statistics.DTO;

namespace SmartFix.Domain.Abstractions;

public interface IStatisticsRepository
{
    Task<RequestsStatsDto> LoadRequestsKpis(DateTime start, DateTime end, CancellationToken cancellationToken);
    
    Task<ClientsStatsDto> LoadClientStats(DateTime start, DateTime end, CancellationToken cancellationToken);
    Task<MastersStatsDto> LoadMasterStats(DateTime start, DateTime end, CancellationToken cancellationToken);
}