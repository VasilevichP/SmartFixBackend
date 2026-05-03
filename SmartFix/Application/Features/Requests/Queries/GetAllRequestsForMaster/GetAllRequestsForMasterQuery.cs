using MediatR;
using SmartFix.Application.Features.Requests.DTO;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Features.Requests.Queries.GetAllRequestsForMaster;

public class GetAllRequestsForMasterQuery: IRequest<List<RequestsBriefDto>>

{
    public Guid MasterId { get; set; }
    public string? Client { get; set; }
    public string? Device { get; set; }
    public RequestStatus? Status { get; set; }
    public int SortOrder { get; set; } = 0;
}