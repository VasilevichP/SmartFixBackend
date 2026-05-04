using MediatR;

namespace SmartFix.Application.Features.Users.Commands.CreateManager;

public class CreateManagerCommand:IRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
}