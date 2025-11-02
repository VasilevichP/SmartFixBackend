using MediatR;

namespace SmartFix.Application.Features.Users.Commands.RegisterClient;

public class RegisterUserCommand : IRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
}