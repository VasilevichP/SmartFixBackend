using SmartFix.Application.Features.Statistics.DTO;

namespace SmartFix.Domain.Abstractions;

public interface IStatisticsRepository
{
    // Task<StatisticsDto> GetStatisticsAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
    Task<GeneralStatsDto> LoadGeneralKpis(DateTime start, DateTime end, CancellationToken cancellationToken);
    Task<ServicesStatsDto> LoadServicesStats(DateTime start, DateTime end, CancellationToken cancellationToken);
    Task<ClientsStatsDto> LoadClientStats(DateTime start, DateTime end, CancellationToken cancellationToken);
    Task<SpecialistsStatsDto> LoadSpecialistStats(DateTime start, DateTime end, CancellationToken cancellationToken);
}