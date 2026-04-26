using MediatR;
using SmartFix.Application.Features.Requests.DTO;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Features.Requests.Queries.GetAllRequestsForManager;

public class GetAllRequestsQuery: IRequest<List<RequestsBriefDto>>

{
    public string? Client { get; set; }
    public string? Device { get; set; }
    public RequestStatus? Status { get; set; }
    public int SortOrder { get; set; } = 0;
}