using MediatR;

namespace SmartFix.Application.Features.Manufacturers.Commands.UpdateManufacturer;

public class UpdateManufacturerCommand: IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}