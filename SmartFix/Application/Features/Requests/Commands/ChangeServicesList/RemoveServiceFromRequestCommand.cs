using MediatR;

namespace SmartFix.Application.Features.Requests.Commands.ChangeServicesList;

public class RemoveServiceFromRequestCommand : IRequest
{
    public Guid RequestId { get; set; }
    public Guid ServiceId { get; set; }
}