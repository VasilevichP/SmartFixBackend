using MediatR;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Features.Clients.Commands.UpdateProfileByManager;

public class UpdateProfileByManagerCommand: IRequest
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public ClientStatus Status { get; set; }
    public int PersonalDiscount { get; set; }
}