using MediatR;

namespace SmartFix.Application.Features.Users.Commands.RegisterClient;

public class RegisterUserCommand : IRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
}