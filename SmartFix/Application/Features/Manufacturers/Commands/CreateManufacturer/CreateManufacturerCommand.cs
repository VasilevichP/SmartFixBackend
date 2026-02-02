using MediatR;

namespace SmartFix.Application.Features.Manufacturers.Commands.CreateManufacturer;

public class CreateManufacturerCommand: IRequest
{
    public string Name { get; set; }
}