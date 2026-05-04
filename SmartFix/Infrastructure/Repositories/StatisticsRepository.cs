using Microsoft.EntityFrameworkCore;
using SmartFix.Application.Features.Statistics.DTO;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.ValueObjects;
using SmartFix.Infrastructure.Persistence;

namespace SmartFix.Infrastructure.Repositories;

public class StatisticsRepository : IStatisticsRepository
{
    private readonly AppDbContext _context;

    public StatisticsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<GeneralStatsDto> LoadGeneralKpis(DateTime start, DateTime end, CancellationToken ct)
    {
        var stats = new GeneralStatsDto();
        var query = _context.Requests.AsNoTracking().Where(r => r.CreatedAt >= start && r.CreatedAt <= end);

        stats.NewRequestsCount = await query.CountAsync(ct);
        stats.ClosedRequestsCount = await _context.Requests.AsNoTracking()
            .Where(r => r.ClosedAt >= start && r.ClosedAt <= end).CountAsync();

        var allReviews = _context.Reviews.AsNoTracking();
        if (await allReviews.AnyAsync(ct))
        {
            stats.AverageRating = Math.Round(await allReviews.AverageAsync(r => r.Rating, ct), 1);
        }

        var closedRequests = await _context.Requests.AsNoTracking()
            .Where(r => r.ClosedAt >= start && r.ClosedAt <= end)
            .Select(r => new { Start = r.CreatedAt, End = r.ClosedAt.Value })
            .ToListAsync(ct);

        if (closedRequests.Any())
        {
            double totalHours = closedRequests.Sum(r => (r.End - r.Start).TotalHours);
            stats.AvgRepairTimeHours = Math.Round(totalHours / closedRequests.Count, 1);
        }

        var dynamics = await query
            .GroupBy(r => r.CreatedAt.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .OrderBy(x => x.Date)
            .ToListAsync(ct);
        stats.RequestsDynamics = dynamics
            .Select(d => new DateValueDto { Date = d.Date.ToString("dd.MM"), Value = d.Count }).ToList();

        var statusData = await _context.Requests.AsNoTracking()
            .GroupBy(r => r.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync(ct);

        stats.StatusDistribution = statusData.Select(s => new LabelValueDto
        {
            Label = s.Status.ToString(),
            Value = s.Count
        }).ToList();


        return stats;
    }
    
    public async Task<Dictionary<DateTime, int>> GetDailyRequestsCountAsync(DateTime start, DateTime end, CancellationToken ct)
    {
        return await _context.Requests
            .AsNoTracking()
            .Where(r => r.CreatedAt >= start && r.CreatedAt <= end)
            .GroupBy(r => r.CreatedAt.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Date, x => x.Count, ct);
    }

    public async Task<ServicesStatsDto> LoadServicesStats(DateTime start, DateTime end, CancellationToken ct)
    {
        var stats = new ServicesStatsDto();

        // var totalRevenue = await _context.Requests
        //     .AsNoTracking()
        //     .Where(r => r.ClosedAt >= start && r.ClosedAt <= end && r.Price != null)
        //     .SumAsync(r => r.Price);
        // stats.TotalRevenue = totalRevenue;
        //
        // var deviceData = await _context.Requests
        //     .GroupBy(r => r.DeviceType.Name)
        //     .Select(g => new { Type = g.Key, Revenue = g.Sum(r => r.Price) })
        //     .ToListAsync(ct);
        // stats.RevenueByDeviceType =
        //     deviceData.Select(d => new LabelValueDto { Label = d.Type, Value = (double)d.Revenue }).ToList();

        return stats;
    }

    public async Task<ClientsStatsDto> LoadClientStats(DateTime start, DateTime end, CancellationToken ct)
    {
        var stats = new ClientsStatsDto();
        // stats.TotalClients = await _context.Clients.CountAsync(u => u.Role == Role.Client, ct);
        //
        // var returningClients = await _context.Requests
        //     .AsNoTracking()
        //     .GroupBy(r => r.ClientId)
        //     .Where(g => g.Count() > 1)
        //     .CountAsync(ct);
        //
        // stats.ReturningClientsCount = returningClients;

        return stats;
    }

    public async Task<SpecialistsStatsDto> LoadSpecialistStats(DateTime start, DateTime end, CancellationToken ct)
    {
        var stats = new SpecialistsStatsDto();
        // var specialists = await _context.Specialists
        //     .AsNoTracking()
        //     .ToListAsync(ct);
        //
        // var resultList = new List<SpecialistPerformanceDto>();
        //
        // foreach (var spec in specialists)
        // {
        //     var specRequests = _context.Requests
        //         .AsNoTracking()
        //         .Where(r => r.MasterId == spec.Id && r.ClosedAt >= start && r.ClosedAt <= end);
        //     var closedCount = await specRequests
        //         .CountAsync(ct);
        //
        //     var activeCount = await _context.Requests
        //         .CountAsync(r => r.MasterId == spec.Id
        //                          && r.Status != RequestStatus.Closed
        //                          && r.Status != RequestStatus.Cancelled, ct);
        //
        //     double avgTime = 0;
        //
        //     if (closedCount > 0)
        //     {
        //         avgTime = await specRequests
        //             .AverageAsync(r => EF.Functions.DateDiffHour(r.CreatedAt, r.ClosedAt.Value), ct);
        //     }
        //
        //     resultList.Add(new SpecialistPerformanceDto
        //     {
        //         Name = spec.Name,
        //         ClosedCount = closedCount,
        //         InProgressCount = activeCount,
        //         AvgRepairTime = Math.Round(avgTime, 1)
        //     });
        // }
        //
        // stats.Performance = resultList
        //     .OrderByDescending(x => x.ClosedCount)
        //     .ToList();

        return stats;
    }
}