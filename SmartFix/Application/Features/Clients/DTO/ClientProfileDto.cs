using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Features.Clients.DTO;

public class ClientProfileDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public ClientStatus? Status { get; set; }
    
    public int? PersonalDiscount { get; set; }
}