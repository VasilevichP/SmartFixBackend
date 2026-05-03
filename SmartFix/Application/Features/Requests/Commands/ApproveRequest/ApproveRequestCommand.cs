using MediatR;

namespace SmartFix.Application.Features.Requests.Commands.ApproveRequest;

public class ApproveRequestCommand:IRequest
{
    public Guid Id { get; set; }
}