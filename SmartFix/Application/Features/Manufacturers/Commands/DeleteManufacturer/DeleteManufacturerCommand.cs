using MediatR;

namespace SmartFix.Application.Features.Manufacturers.Commands.DeleteManufacturer;

public class DeleteManufacturerCommand:IRequest
{
    public Guid Id { get; set; }
}