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

        return await _repository.LoadGeneralKpis(startDate, endDate, cancellationToken);
    }
}