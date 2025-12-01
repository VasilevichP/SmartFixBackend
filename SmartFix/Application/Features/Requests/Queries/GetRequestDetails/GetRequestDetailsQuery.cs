using MediatR;
using SmartFix.Application.Features.Requests.DTO;

namespace SmartFix.Application.Features.Requests.Queries.GetRequestDetails;

public class GetRequestDetailsQuery: IRequest<RequestDetailsDto>
{
    public Guid RequestId { get; set; }
}