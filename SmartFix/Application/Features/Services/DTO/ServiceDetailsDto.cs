using SmartFix.Application.Features.Reviews.DTO;

namespace SmartFix.Application.Features.Services.DTO;

public class ServiceDetailsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int WarrantyPeriod { get; set; }
    public string CategoryName { get; set; }
    
    public string DeviceTypeName { get; set; }

    public string? DeviceModelName { get; set; }
    public string? ManufacturerName { get; set; }
    
    public bool IsAvailable { get; set; }
    
    public double AverageRating { get; set; }
    public int ReviewsCount { get; set; }
    public List<ReviewDto> Reviews { get; set; } = new();
}