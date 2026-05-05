using MediatR;
using SmartFix.Application.Common.Extension;
using SmartFix.Application.Features.Statistics.DTO;
using SmartFix.Application.Helpers;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Statistics.Queries.GetMastersStatistics;

public class GetMastersStatisticsQueryHandler: IRequestHandler<GetMastersStatisticsQuery, MastersStatsDto>
{
    private readonly IStatisticsRepository _repository;

    public GetMastersStatisticsQueryHandler(IStatisticsRepository repository)
    {
        _repository = repository;
    }

    public async Task<MastersStatsDto> Handle(GetMastersStatisticsQuery request, CancellationToken cancellationToken)
    {
        var (startDate, endDate) = DateRangeCalculator.CalculateDateRange(request.Period, request.From, request.To);
        return await _repository.LoadMasterStats(startDate, endDate, cancellationToken);
    }
}