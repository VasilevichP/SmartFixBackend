using MediatR;

namespace SmartFix.Application.Features.Services.Commands.UpdateService;

public class UpdateServiceCommand : IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int WarrantyPeriod { get; set; }
    public Guid CategoryId { get; set; }
    public Guid DeviceTypeId { get; set; }
    public Guid? ManufacturerId { get; set; } 
    public Guid? DeviceModelId { get; set; }
    public Boolean IsAvailable { get; set; }
}