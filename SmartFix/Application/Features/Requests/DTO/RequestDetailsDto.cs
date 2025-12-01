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
    
    // Информация об услуге
    public string ServiceName { get; set; }
    public decimal Price { get; set; }
    public int WarrantyPeriod { get; set; }
    // Даты
    public DateTime CreatedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    
    // Участники
    public string ClientName { get; set; }
    public string? SpecialistName { get; set; }
    
    // Фотографии (список ссылок)
    public List<string> PhotoPaths { get; set; } = new();
    
    // История статусов
    public List<StatusHistoryDto> History { get; set; } = new();
}