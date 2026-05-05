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


    public async Task<RequestsStatsDto> LoadRequestsKpis(DateTime start, DateTime end,
        CancellationToken cancellationToken)
    {
        var baseQuery = _context.Requests.AsNoTracking().Where(r => r.CreatedAt >= start && r.CreatedAt <= end);
        var closedQuery = baseQuery.Where(r => r.Status == RequestStatus.Closed);

        int totalRequests = await baseQuery.CountAsync(cancellationToken);
        int closedRequests = await closedQuery.CountAsync(cancellationToken);
        int cancelledRequests = await baseQuery.CountAsync(r => r.Status == RequestStatus.Cancelled, cancellationToken);
        decimal totalRevenue = await closedQuery.SumAsync(r => r.FinalPrice, cancellationToken);

        var byStatus = await baseQuery.GroupBy(r => r.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() }).ToListAsync(cancellationToken);

        var byType = await baseQuery.GroupBy(r => r.Type)
            .Select(g => new { Type = g.Key, Count = g.Count() }).ToListAsync(cancellationToken);

        var byDeviceType = await baseQuery.GroupBy(r => r.DeviceType.Name)
            .Select(g => new { Device = g.Key, Count = g.Count() }).ToListAsync(cancellationToken);

        var byDay = await baseQuery.GroupBy(r => r.CreatedAt.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() }).ToListAsync(cancellationToken);

        var repairTimes = await closedQuery.Select(r => new { r.CreatedAt, r.ClosedAt }).ToListAsync(cancellationToken);

        double avgHours = 0;
        if (repairTimes.Any())
        {
            avgHours = repairTimes.Average(r => (r.ClosedAt!.Value - r.CreatedAt).TotalHours);
        }

        return new RequestsStatsDto
        {
            TotalRequests = totalRequests,
            ClosedRequests = closedRequests,
            CancelledRequests = cancelledRequests,
            TotalRevenue = totalRevenue,
            AverageCheck = closedRequests > 0 ? totalRevenue / closedRequests : 0,
            AverageRepairTimeHours = Math.Round(avgHours, 1),

            RequestsByStatus = byStatus.ToDictionary(x => x.Status.ToString(), x => x.Count),
            RequestsByType = byType.ToDictionary(x => x.Type.ToString(), x => x.Count),
            RequestsByDeviceType = byDeviceType.ToDictionary(x => x.Device ?? "Неизвестно", x => x.Count),
            RequestsByDay = byDay.ToDictionary(x => x.Date.ToString("yyyy-MM-dd"), x => x.Count)
        };
    }

    public async Task<ClientsStatsDto> LoadClientStats(DateTime start, DateTime end,
        CancellationToken cancellationToken)
    {
        var newClientsCount = await _context.Requests
            .AsNoTracking()
            .Where(r => r.CreatedAt >= start && r.CreatedAt <= end)
            .Select(r => r.ClientId)
            .Distinct()
            .CountAsync(clientId => !_context.Requests.Any(old => old.ClientId == clientId && old.CreatedAt < start),
                cancellationToken);

        var returningRequestsCount = await _context.Requests
            .AsNoTracking()
            .Where(r => r.CreatedAt >= start && r.CreatedAt <= end)
            .CountAsync(
                r => _context.Requests.Count(all => all.ClientId == r.ClientId && all.Status == RequestStatus.Closed) >
                     1, cancellationToken);

        var reviewsInPeriod = _context.Reviews.AsNoTracking();

        double avgRating = await reviewsInPeriod.AnyAsync(cancellationToken)
            ? await reviewsInPeriod.AverageAsync(r => r.Rating, cancellationToken)
            : 0.0;

        var distribution = await reviewsInPeriod.GroupBy(r => r.Rating)
            .Select(g => new { Rating = g.Key, Count = g.Count() }).ToListAsync(cancellationToken);

        return new ClientsStatsDto
        {
            NewClientsCount = newClientsCount,
            ReturningClientRequestsCount = returningRequestsCount,
            AverageRating = Math.Round(avgRating, 2),
            RatingDistribution = distribution.ToDictionary(x => x.Rating, x => x.Count)
        };
    }

    public async Task<MastersStatsDto> LoadMasterStats(DateTime start, DateTime end,
        CancellationToken cancellationToken)
    {
        var requestsInPeriod = _context.Requests.AsNoTracking()
            .Include(r => r.Master)
            .Where(r => r.CreatedAt >= start && r.CreatedAt <= end && r.MasterId != null);

        var revenueData = await requestsInPeriod
            .Where(r => r.Status == RequestStatus.Closed)
            .GroupBy(r => new { r.MasterId, r.Master.Name })
            .Select(g => new
            {
                MasterName = g.Key.Name,
                TotalRevenue = g.Sum(r => r.FinalPrice),
                ClosedCount = g.Count()
            })
            .ToListAsync(cancellationToken);

        var topMaster = revenueData.OrderByDescending(x => x.ClosedCount).FirstOrDefault();

        var rejectionData = await requestsInPeriod
            .Where(r => r.Status == RequestStatus.Closed || r.Status == RequestStatus.Cancelled)
            .GroupBy(r => r.Master.Name)
            .Select(g => new
            {
                MasterName = g.Key,
                TotalHandled = g.Count(),
                CancelledCount = g.Count(r => r.Status == RequestStatus.Cancelled)
            })
            .ToListAsync(cancellationToken);

        var rejectionRateDict = rejectionData.ToDictionary(
            x => x.MasterName!,
            x => Math.Round((double)x.CancelledCount / x.TotalHandled * 100, 1)
        );

        var diagTimes = await requestsInPeriod
            .Where(r => r.DiagnosticResult != null)
            .Select(r => new
            {
                Start = r.CreatedAt,
                End = r.StatusHistories
                    .Where(h => h.Status == RequestStatus.Pending || h.Status == RequestStatus.InProgress)
                    .Select(h => h.Timestamp).FirstOrDefault()
            })
            .ToListAsync(cancellationToken);

        double avgDiagTime = 0;
        var validDiagTimes = diagTimes.Where(x => x.End != default).ToList();
        if (validDiagTimes.Any())
        {
            avgDiagTime = validDiagTimes.Average(x => (x.End - x.Start).TotalHours);
        }

        return new MastersStatsDto
        {
            ActiveMastersCount = revenueData.Count,
            TopMasterName = topMaster?.MasterName ?? "Нет данных",
            AverageDiagnosticTimeHours = Math.Round(avgDiagTime, 1),
            RevenueByMaster = revenueData.ToDictionary(x => x.MasterName!, x => x.TotalRevenue),
            RejectionRateByMaster = rejectionRateDict
        };
    }
}