using MediatR;

namespace SmartFix.Application.Features.DeviceModels.Commands.DeleteDeviceModel;

public class DeleteDeviceModelCommand:IRequest
{
    public Guid Id { get; set; }
}