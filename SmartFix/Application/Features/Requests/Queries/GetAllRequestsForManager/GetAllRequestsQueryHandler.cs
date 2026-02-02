using MediatR;
using SmartFix.Application.Features.Requests.DTO;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Requests.Queries.GetAllRequestsForManager;

public class GetAllRequestsQueryHandler: IRequestHandler<GetAllRequestsQuery, List<RequestsBriefDto>>
{
    private readonly IRequestRepository _requestRepository;

    public GetAllRequestsQueryHandler(IRequestRepository requestRepository)
    {
        _requestRepository = requestRepository;
    }

    public async Task<List<RequestsBriefDto>> Handle(GetAllRequestsQuery request, CancellationToken cancellationToken)
    {
        var requests = await _requestRepository.GetAllAsync(cancellationToken);

        return requests.Select(r => new RequestsBriefDto
        {
            Id = r.Id,
            ClientName = r.Client.Name,
            ServiceName = r.Service.Name,
            CreatedAt = r.CreatedAt,
            Status = r.Status,
            SpecialistName = r.Specialist?.FullName ?? "Не назначен"
        }).ToList();
    }
}