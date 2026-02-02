using MediatR;

namespace SmartFix.Application.Features.Requests.Commands.CreateRequest;

public class CreateRequestCommand : IRequest<Guid>
{
    public Guid ClientId { get; set; }
    public string Description { get; set; }
    public string DeviceSerialNumber { get; set; }
    public Guid? ServiceId { get; set; }
    public Guid DeviceTypeId { get; set; }
    public Guid? DeviceModelId { get; set; }
    public string DeviceModelName { get; set; }

    public List<IFormFile>? Photos { get; set; }
}