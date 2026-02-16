using MediatR;
using SmartFix.Application.Common.Extension;
using SmartFix.Application.Features.Statistics.DTO;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Statistics.Queries.GetServicesStatistics;

public class GetServicesStatisticsQueryHandler: IRequestHandler<GetServicesStatisticsQuery, ServicesStatsDto>
{
    private readonly IStatisticsRepository _repository;

    public GetServicesStatisticsQueryHandler(IStatisticsRepository repository)
    {
        _repository = repository;
    }

    public async Task<ServicesStatsDto> Handle(GetServicesStatisticsQuery request, CancellationToken cancellationToken)
    {
        var (startDate, endDate) = request.CalculateDateRange();

        return await _repository.LoadServicesStats(startDate, endDate, cancellationToken);
    }
}