using MediatR;

namespace SmartFix.Application.Features.DeviceTypes.Commands.DeleteDeviceType;

public class DeleteDeviceTypeCommand : IRequest
{
    public Guid Id { get; set; }
}