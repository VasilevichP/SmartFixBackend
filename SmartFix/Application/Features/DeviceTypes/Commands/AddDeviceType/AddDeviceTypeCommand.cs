using MediatR;

namespace SmartFix.Application.Features.DeviceTypes.Commands.AddDeviceType;

public class AddDeviceTypeCommand:IRequest
{
    public string Name { get; set; }
}