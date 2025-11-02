namespace SmartFix.Application.Features.Services.DTO;

public class ServiceDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string CategoryName { get; set; }
    public bool IsAvailable { get; set; }
}