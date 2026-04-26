using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Features.Clients.DTO;

public class ClientBriefDto
{
    public Guid Id { get; set; }
    public string? Phone { get; set; }
    public ClientStatus? Status { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}