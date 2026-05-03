using System.ComponentModel.DataAnnotations;
using MediatR;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Features.Requests.Commands.CreateRequest;

public class CreateRequestCommand : IRequest<Guid>
{
    public Guid ClientId { get; set; }
    
    public RequestType Type { get; set; }
    public Guid DeviceTypeId { get; set; }
    public Guid? DeviceModelId { get; set; }
    public string DeviceModelName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactName { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    
    public string? PromoCodeCode { get; set; }
    public string? FieldAddress { get; set; }
    public DateTime? ScheduledTime { get; set; }
    public Guid? ParentRequestId { get; set; }
    public List<Guid> ServiceIds { get; set; } = new();
    public List<IFormFile>? Photos { get; set; }
}