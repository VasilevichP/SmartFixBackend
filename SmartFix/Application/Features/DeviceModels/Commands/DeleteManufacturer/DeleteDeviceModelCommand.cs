using MediatR;

namespace SmartFix.Application.Features.Manufacturers.Commands.DeleteManufacturer;

public class DeleteDeviceModelCommand:IRequest
{
    public Guid Id { get; set; }
}