using MediatR;

namespace SmartFix.Application.Features.DeviceModels.Commands.CreateDeviceModel;


public class CreateDeviceModelCommand: IRequest
{
    public string Name { get; set; }
    public Guid ManufacturerId { get; set; }
    public Guid DeviceTypeId { get; set; }
}