using MediatR;

namespace SmartFix.Application.Features.DeviceTypes.Commands.UpdateDeviceType;

public class UpdateDeviceTypeCommand: IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}