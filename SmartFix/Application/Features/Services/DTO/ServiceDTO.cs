using SmartFix.Application.Features.Reviews.DTO;

namespace SmartFix.Application.Features.Services.DTO;

public class ServiceDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; }
    public Guid DeviceTypeId { get; set; }
    public string DeviceTypeName { get; set; }

    public Guid? DeviceModelId { get; set; }
    public string? DeviceModelName { get; set; }
    
    public Guid? ManufacturerId { get; set; }
    public string? ManufacturerName { get; set; } 
    public bool IsAvailable { get; set; }
}