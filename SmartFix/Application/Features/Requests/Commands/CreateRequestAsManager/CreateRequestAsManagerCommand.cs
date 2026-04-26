using MediatR;
using SmartFix.Application.Features.Services.DTO;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Features.Requests.Commands.CreateRequestAsManager;

public class CreateRequestAsManagerCommand: IRequest<Guid>
{
    public Guid? ClientId { get; set; } 
   
    public string ContactName { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    
    public RequestType Type { get; set; }
    public Guid DeviceTypeId { get; set; }
    public Guid? DeviceModelId { get; set; }
    public string DeviceModelName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    
    public string? DeviceAppearance { get; set; } = string.Empty;
    public string? DevicePackage { get; set; } = string.Empty;
    public string? FieldAddress { get; set; }
    public DateTime? ScheduledTime { get; set; }
    public Guid? ParentRequestId { get; set; }
    
    public List<Guid> ServiceIds { get; set; } = new();
    public List<CustomServiceForRequestDto> CustomServices { get; set; } = new();
}