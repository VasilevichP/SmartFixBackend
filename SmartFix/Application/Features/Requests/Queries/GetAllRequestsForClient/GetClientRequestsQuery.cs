using MediatR;
using SmartFix.Application.Features.Requests.DTO;

namespace SmartFix.Application.Features.Requests.Queries.GetAllRequestsForClient;

public class GetClientRequestsQuery : IRequest<List<RequestsBriefDto>>
{
    public Guid ClientId { get; set; }
}