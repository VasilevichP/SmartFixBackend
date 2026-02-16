using MediatR;
using SmartFix.Application.Common.Extension;
using SmartFix.Application.Features.Statistics.DTO;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Statistics.Queries.GetSpecialistsStatistics;

public class GetSpecialistsStatisticsQueryHandler: IRequestHandler<GetSpecialistsStatisticsQuery, SpecialistsStatsDto>
{
    private readonly IStatisticsRepository _repository;

    public GetSpecialistsStatisticsQueryHandler(IStatisticsRepository repository)
    {
        _repository = repository;
    }

    public async Task<SpecialistsStatsDto> Handle(GetSpecialistsStatisticsQuery request, CancellationToken cancellationToken)
    {
        var (startDate, endDate) = request.CalculateDateRange();

        return await _repository.LoadSpecialistStats(startDate, endDate, cancellationToken);
    }
}