using SmartFix.Domain.Aggregates;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Features.Requests.DTO;

public class RequestDetailsDto
{
    public Guid Id { get; set; }
    public RequestType Type { get; set; }
    public RequestStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ClosedAt { get; set; }

    public Guid ClientId { get; set; }
    public string ContactName { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;

    public string DeviceTypeName { get; set; } = string.Empty;
    public string DeviceModelName { get; set; } = string.Empty;
    public string DeviceSerialNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string? DiagnosticResult { get; set; }
    public string? DeviceAppearance { get; set; }
    public string? DevicePackage { get; set; }

    public string? FieldAddress { get; set; }
    public DateTime? ScheduledTime { get; set; }
    public Guid? ParentRequestId { get; set; }

    public decimal BasePrice { get; set; }
    public decimal FinalPrice { get; set; }

    public Guid? MasterId { get; set; }
    public string? MasterName { get; set; }
    public List<string> PhotoPaths { get; set; } = new();
    public List<RequestServiceDto> Services { get; set; } = new();
    public List<RequestDiscountDto> AppliedDiscounts { get; set; } = new();
    public List<StatusHistoryDto> StatusHistories { get; set; } = new();
}