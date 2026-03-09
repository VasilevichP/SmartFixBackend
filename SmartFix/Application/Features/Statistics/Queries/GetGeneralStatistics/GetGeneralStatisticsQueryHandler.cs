using MediatR;
using SmartFix.Application.Common.Extension;
using SmartFix.Application.Features.Statistics.DTO;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Statistics.Queries.GetGeneralStatistics;

public class GetGeneralStatsQueryHandler : IRequestHandler<GetGeneralStatisticsQuery, GeneralStatsDto>
{
    private readonly IStatisticsRepository _repository;

    public GetGeneralStatsQueryHandler(IStatisticsRepository repository)
    {
        _repository = repository;
    }

    public async Task<GeneralStatsDto> Handle(GetGeneralStatisticsQuery request, CancellationToken cancellationToken)
    {
        var (startDate, endDate) = request.CalculateDateRange();
        var stats = await _repository.LoadGeneralKpis(startDate, endDate, cancellationToken);
        var dbData = await _repository.GetDailyRequestsCountAsync(startDate, endDate, cancellationToken);

        var fullList = new List<DateValueDto>();
    
        for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
        {
            fullList.Add(new DateValueDto
            {
                Date = date.ToString("dd.MM"),
                Value = dbData.GetValueOrDefault(date) 
            });
        }
        stats.RequestsDynamics = fullList;

        return stats;
    }
}