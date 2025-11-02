using MediatR;

namespace SmartFix.Application.Features.Users.Commands.AuthorizeUser;

public class AuthorizeUserCommand : IRequest<string>
{
    public string Email { get; set; }
    public string Password { get; set; }
}