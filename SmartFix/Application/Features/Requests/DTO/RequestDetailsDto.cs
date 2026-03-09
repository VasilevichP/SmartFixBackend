using SmartFix.Domain.Aggregates;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Features.Requests.DTO;

public class RequestDetailsDto
{
    public Guid Id { get; set; }
    public RequestStatus Status { get; set; }
    public string StatusName => Status.ToString();
    
    // Информация об устройстве
    public string DeviceType { get; set; }
    public string DeviceModel { get; set; }
    public string DeviceSerialNumber { get; set; }
    public string Description { get; set; }
    public decimal? Price { get; set; }
    public string? CancellationReason { get; set; }
    public string? DiagnosticResult { get; set; }
    public string? Appearance { get; set; }
    public string? Package { get; set; }
    // Даты
    public DateTime CreatedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    
    // Участники
    public Guid ClientId { get; set; }
    public string ClientEmail { get; set; }
    public string ClientPhone { get; set; }
    public string ClientName { get; set; }
    public bool IsCourierDelivery {get; set;}
    public string? Address {get; set;}
    public decimal DeliveryCost {get; set;}
    
    public Guid? SpecialistId { get; set; }

    public string? SpecialistName { get; set; }
    public List<RequestServiceDto> Services { get; set; } = new();
    public List<string> PhotoPaths { get; set; } = new();
    public List<StatusHistoryDto> History { get; set; } = new();
}