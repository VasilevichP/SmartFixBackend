namespace SmartFix.Application.Features.Requests.DTO;

public class RequestServiceDto
{
    public Guid Id { get; set; }
    public Guid? ServiceId { get; set; }
    public string ServiceName { get; set; }
    public decimal Price { get; set; }
}