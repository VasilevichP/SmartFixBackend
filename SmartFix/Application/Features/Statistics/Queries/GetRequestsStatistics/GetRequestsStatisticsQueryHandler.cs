using MediatR;
using SmartFix.Application.Common.Extension;
using SmartFix.Application.Features.Statistics.DTO;
using SmartFix.Application.Helpers;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Statistics.Queries.GetRequestsStatistics;

public class GetGeneralStatsQueryHandler : IRequestHandler<GetRequestsStatisticsQuery, RequestsStatsDto>
{
    private readonly IStatisticsRepository _repository;

    public GetGeneralStatsQueryHandler(IStatisticsRepository repository)
    {
        _repository = repository;
    }

    public async Task<RequestsStatsDto> Handle(GetRequestsStatisticsQuery request, CancellationToken cancellationToken)
    {
        var (startDate, endDate) = DateRangeCalculator.CalculateDateRange(request.Period, request.From, request.To);
        return await _repository.LoadRequestsKpis(startDate, endDate, cancellationToken);
    }
}