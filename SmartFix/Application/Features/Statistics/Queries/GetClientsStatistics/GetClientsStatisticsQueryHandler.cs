using MediatR;
using SmartFix.Application.Common.Extension;
using SmartFix.Application.Features.Statistics.DTO;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Statistics.Queries.GetClientsStatistics;

public class GetClientsStatisticsQueryHandler: IRequestHandler<GetClientsStatisticsQuery, ClientsStatsDto>
{
    private readonly IStatisticsRepository _repository;

    public GetClientsStatisticsQueryHandler(IStatisticsRepository repository)
    {
        _repository = repository;
    }

    public async Task<ClientsStatsDto> Handle(GetClientsStatisticsQuery request, CancellationToken cancellationToken)
    {
        var (startDate, endDate) = request.CalculateDateRange();

        return await _repository.LoadClientStats(startDate, endDate, cancellationToken);
    }
}