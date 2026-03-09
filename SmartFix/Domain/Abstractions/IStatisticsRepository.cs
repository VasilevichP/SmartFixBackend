using SmartFix.Application.Features.Statistics.DTO;

namespace SmartFix.Domain.Abstractions;

public interface IStatisticsRepository
{
    Task<GeneralStatsDto> LoadGeneralKpis(DateTime start, DateTime end, CancellationToken cancellationToken);

    public Task<Dictionary<DateTime, int>> GetDailyRequestsCountAsync(DateTime start, DateTime end,
        CancellationToken ct);
    Task<ServicesStatsDto> LoadServicesStats(DateTime start, DateTime end, CancellationToken cancellationToken);
    Task<ClientsStatsDto> LoadClientStats(DateTime start, DateTime end, CancellationToken cancellationToken);
    Task<SpecialistsStatsDto> LoadSpecialistStats(DateTime start, DateTime end, CancellationToken cancellationToken);
}