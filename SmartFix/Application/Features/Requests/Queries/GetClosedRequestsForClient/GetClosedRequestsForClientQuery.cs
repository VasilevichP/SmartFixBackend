using MediatR;
using SmartFix.Application.Features.Requests.DTO;

namespace SmartFix.Application.Features.Requests.Queries.GetClosedRequestsForClient;

public class GetClosedRequestsForClientQuery:IRequest<List<RequestForSelectDto>>
{
    public Guid ClientId { get; set; }
}