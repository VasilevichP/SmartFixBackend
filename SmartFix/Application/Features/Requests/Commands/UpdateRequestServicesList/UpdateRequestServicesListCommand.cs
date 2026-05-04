using MediatR;
using SmartFix.Application.Features.Requests.DTO;

namespace SmartFix.Application.Features.Requests.Commands.UpdateRequestServicesList;

public class UpdateRequestServicesListCommand: IRequest
{
    public Guid RequestId { get; set; }
    public List<ServiceItemDto> Services { get; set; } = new();
}