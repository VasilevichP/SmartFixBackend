using MediatR;

namespace SmartFix.Application.Features.Requests.Commands.RejectRequest;

public class RejectRequestCommand: IRequest
{
    public Guid RequestId { get; set; }
}
