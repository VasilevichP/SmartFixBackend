using System.ComponentModel.DataAnnotations;
using MediatR;

namespace SmartFix.Application.Features.Requests.Commands.CreateRequest;

public class CreateRequestCommand : IRequest<Guid>
{
    public Guid ClientId { get; set; }

    [EmailAddress(ErrorMessage = "Некорректный формат Email")]
    public string ContactEmail { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Некорректный формат номера телефона")]
    public string ContactPhoneNumber { get; set; } = string.Empty;

    public string ContactName { get; set; } = string.Empty;
    public bool IsCourierDelivery { get; set; }
    public string? Address { get; set; }
    public decimal? Price { get; set; }
    public string Description { get; set; }
    public string DeviceSerialNumber { get; set; }
    public Guid? ServiceId { get; set; }
    public Guid DeviceTypeId { get; set; }
    public Guid? DeviceModelId { get; set; }
    public string DeviceModelName { get; set; }

    public List<IFormFile>? Photos { get; set; }
}