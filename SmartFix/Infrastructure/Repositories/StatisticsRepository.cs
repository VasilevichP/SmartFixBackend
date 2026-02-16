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
        stats.ClosedRequestsCount = await query.CountAsync(r => r.Status == RequestStatus.Closed, ct);

        var allReviews = _context.Reviews.AsNoTracking();
        if (await allReviews.AnyAsync(ct))
        {
            stats.AverageRating = Math.Round(await allReviews.AverageAsync(r => r.Rating, ct), 1);
        }

        // Среднее время ремонта (только по закрытым в этом периоде)
        var closedRequests = await query
            .Where(r => r.Status == RequestStatus.Closed && r.ClosedAt.HasValue)
            .Select(r => new { Start = r.CreatedAt, End = r.ClosedAt.Value })
            .ToListAsync(ct);

        if (closedRequests.Any())
        {
            double totalHours = closedRequests.Sum(r => (r.End - r.Start).TotalHours);
            stats.AvgRepairTimeHours = Math.Round(totalHours / closedRequests.Count, 1);
        }

        // Динамика
        var dynamics = await query
            .GroupBy(r => r.CreatedAt.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .OrderBy(x => x.Date)
            .ToListAsync(ct);
        stats.RequestsDynamics = dynamics
            .Select(d => new DateValueDto { Date = d.Date.ToString("dd.MM"), Value = d.Count }).ToList();

        // Статусы (Перевод на русский)
        var statusData = await query
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

    public async Task<ServicesStatsDto> LoadServicesStats(DateTime start, DateTime end, CancellationToken ct)
    {
        var stats = new ServicesStatsDto();

        var totalRevenue = await _context.Requests
            .AsNoTracking()
            .Where(r => r.CreatedAt >= start && r.CreatedAt <= end && r.Price != null)
            .SumAsync(r => r.Price);
        stats.TotalRevenue = totalRevenue ?? 0;

        // Топ 5 услуг по количеству заявок
        var topServices = await _context.Requests
            .AsNoTracking()
            .Where(r => r.CreatedAt >= start && r.CreatedAt <= end && r.ServiceId != null)
            .GroupBy(r => r.Service.Name)
            .Select(g => new { Name = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .ToListAsync(ct);
        stats.TopServices = topServices.Select(x => new LabelValueDto { Label = x.Name, Value = x.Count }).ToList();

        // Выручка по типам устройств
        var deviceData = await _context.Requests
            .GroupBy(r => r.DeviceType.Name) // Группируем по названию типа (Смартфон, Ноутбук)
            .Select(g => new { Type = g.Key, Revenue = g.Sum(r => r.Price ?? 0) })
            .ToListAsync(ct);
        stats.RevenueByDeviceType =
            deviceData.Select(d => new LabelValueDto { Label = d.Type, Value = (double)d.Revenue }).ToList();

        return stats;
    }

    public async Task<ClientsStatsDto> LoadClientStats(DateTime start, DateTime end, CancellationToken ct)
    {
        var stats = new ClientsStatsDto();
        stats.TotalClients = await _context.Users.CountAsync(u => u.Role == Role.Client, ct);

        // Повторные обращения (Retention)
        // Считаем клиентов, у которых > 1 заявки (за всё время, так как это характеристика базы)
        var returningClients = await _context.Requests
            .AsNoTracking()
            .GroupBy(r => r.ClientId)
            .Where(g => g.Count() > 1)
            .CountAsync(ct);

        stats.ReturningClientsCount = returningClients;

        return stats;
    }

    public async Task<SpecialistsStatsDto> LoadSpecialistStats(DateTime start, DateTime end, CancellationToken ct)
    {
        var stats = new SpecialistsStatsDto();
        var specialists = await _context.Specialists
            .AsNoTracking()
            .ToListAsync(ct);

        var resultList = new List<SpecialistPerformanceDto>();

        foreach (var spec in specialists)
        {
            var specRequests = _context.Requests
                .AsNoTracking()
                .Where(r => r.SpecialistId == spec.Id && r.CreatedAt >= start && r.CreatedAt <= end);

            var closedCount = await specRequests
                .CountAsync(r => r.Status == RequestStatus.Closed, ct);

            // B. Количество в работе (все активные, не закрытые и не отмененные)
            // Тут берем срез "на данный момент", поэтому дату start/end можно убрать или оставить по желанию
            // (Обычно "В работе" интересно на текущий момент, без привязки к дате создания)
            var activeCount = await _context.Requests
                .CountAsync(r => r.SpecialistId == spec.Id
                                 && r.Status != RequestStatus.Closed
                                 && r.Status != RequestStatus.Cancelled, ct);
    
            double avgTime = 0;

            // Проверяем, есть ли закрытые заявки, чтобы не делить на ноль
            if (closedCount > 0)
            {
                avgTime = await specRequests
                    .Where(r => r.Status == RequestStatus.Closed && r.ClosedAt.HasValue)
                    .AverageAsync(r => EF.Functions.DateDiffHour(r.CreatedAt, r.ClosedAt.Value), ct);
            }

            resultList.Add(new SpecialistPerformanceDto
            {
                Name = spec.Name,
                ClosedCount = closedCount,
                InProgressCount = activeCount,
                AvgRepairTime = Math.Round(avgTime, 1)
            });
        }

        // Сортируем топ по продуктивности
        stats.Performance = resultList
            .OrderByDescending(x => x.ClosedCount)
            .ToList();

        return stats;
    }
}

