using MediatR;

namespace SmartFix.Application.Features.Clients.Commands.CreateProfileByManager;

public class CreateProfileByManagerCommand:IRequest
{
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public int PersonalDiscount { get; set; }
}