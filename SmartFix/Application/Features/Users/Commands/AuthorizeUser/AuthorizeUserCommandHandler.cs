using MediatR;
using Microsoft.AspNetCore.Identity;
using SmartFix.Application.Authentication;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;

namespace SmartFix.Application.Features.Users.Commands.AuthorizeUser;

public class AuthorizeUserCommandHandler: IRequestHandler<AuthorizeUserCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public AuthorizeUserCommandHandler(IUserRepository userRepository, IJwtProvider jwtProvider, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
        _passwordHasher = passwordHasher;
    }

    public async Task<string> Handle(AuthorizeUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null)
        {
            throw new Exception("Неправильный логин или пароль");
        }

        if (!_passwordHasher.VerifyPassword(user.PasswordHash,request.Password))
        {
            throw new Exception("Неправильный логин или пароль");
        }
        
        var token = _jwtProvider.GenerateToken(user);
        
        Console.WriteLine(token);
        return token;
    }
}