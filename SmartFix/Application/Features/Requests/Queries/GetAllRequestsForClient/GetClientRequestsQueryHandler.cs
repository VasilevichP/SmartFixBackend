using MediatR;
using SmartFix.Application.Features.Requests.DTO;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Requests.Queries.GetAllRequestsForClient;

public class GetClientRequestsQueryHandler : IRequestHandler<GetClientRequestsQuery, List<RequestsBriefDto>>
{
    private readonly IRequestRepository _requestRepository;

    public GetClientRequestsQueryHandler(IRequestRepository requestRepository)
    {
        _requestRepository = requestRepository;
    }

    public async Task<List<RequestsBriefDto>> Handle(GetClientRequestsQuery request, CancellationToken cancellationToken)
    {
        var requests = await _requestRepository.GetAllForClientAsync(request.ClientId, cancellationToken);

        return requests.Select(r => new RequestsBriefDto
        {
            Id = r.Id,
            ServiceName = r.Service.Name,
            CreatedAt = r.CreatedAt,
            Status = r.Status
        }).ToList();
    }
}