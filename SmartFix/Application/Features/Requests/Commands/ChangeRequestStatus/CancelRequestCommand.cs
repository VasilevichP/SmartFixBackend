using MediatR;

namespace SmartFix.Application.Features.Requests.Commands.ChangeRequestStatus;

public class CancelRequestCommand:IRequest
{
    public Guid Id { get; set; }
    public string Reason { get; set; }
}