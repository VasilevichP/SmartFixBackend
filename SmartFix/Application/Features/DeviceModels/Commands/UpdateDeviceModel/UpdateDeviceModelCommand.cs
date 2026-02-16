using MediatR;

namespace SmartFix.Application.Features.DeviceModels.Commands.UpdateDeviceModel;

public class UpdateDeviceModelCommand: IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid ManufacturerId { get; set; }
    public Guid DeviceTypeId { get; set; }
}