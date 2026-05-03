namespace SmartFix.Application.Features.Requests.DTO;

public class ServiceItemDto
{
    public Guid? ServiceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int? Warranty { get; set; }
}