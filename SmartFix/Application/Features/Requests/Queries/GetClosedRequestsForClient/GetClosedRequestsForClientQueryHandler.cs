using MediatR;
using SmartFix.Application.Features.Requests.DTO;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Requests.Queries.GetClosedRequestsForClient;

public class GetClosedRequestsForClientQueryHandler:IRequestHandler<GetClosedRequestsForClientQuery, List<RequestForSelectDto>>
{
    private readonly IRequestRepository _requestRepository;

    public GetClosedRequestsForClientQueryHandler(IRequestRepository requestRepository)
    {
        _requestRepository = requestRepository;
    }

    public async Task<List<RequestForSelectDto>> Handle(GetClosedRequestsForClientQuery request, CancellationToken cancellationToken)
    {
        return await _requestRepository.GetClosedForClient(request.ClientId, cancellationToken);
    }
}