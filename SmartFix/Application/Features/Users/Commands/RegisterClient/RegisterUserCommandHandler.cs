using MediatR;
using Microsoft.AspNetCore.Identity;
using SmartFix.Application.Authentication;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;

namespace SmartFix.Application.Features.Users.Commands.RegisterClient;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingUser != null)
        {
            throw new Exception("Пользователь с таким email уже существует.");
        }

        var passwordHash = _passwordHasher.HashPassword(request.Password);

        User user;
        if (await _userRepository.DoesManagerExistAsync(cancellationToken))
        {
            user = User.CreateClient(
                            request.Email,
                            passwordHash,
                            request.FirstName,
                            request.LastName,
                            request.Phone,
                            request.MiddleName
                        );
        }
        else
        {
            user = User.CreateManager(request.Email, passwordHash);
        }

        await _userRepository.AddAsync(user);
    }
}