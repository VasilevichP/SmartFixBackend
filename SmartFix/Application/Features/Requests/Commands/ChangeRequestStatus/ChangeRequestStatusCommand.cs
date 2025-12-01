using MediatR;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Features.Requests.Commands.ChangeRequestStatus;

public class ChangeRequestStatusCommand : IRequest
{
    public Guid RequestId { get; set; }
    public RequestStatus NewStatus { get; set; }
}