using MediatR;

namespace SmartFix.Application.Features.Requests.Commands.ChangeDeviceInfo;

public class ChangeDeviceInfoCommand: IRequest
{
    public Guid Id { get; set; }
    public Guid DeviceTypeId { get; set; }
    public Guid? DeviceModelId { get; set; }
    public string DeviceModelName { get; set; }
    public string DeviceSerialNumber { get; set; }
}