using System.Net;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SmartFix.Application.Authentication;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Domain.Exceptions;

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
            throw new HttpException(HttpStatusCode.BadRequest, "Пользователь с таким email уже существует.");
        }

        var passwordHash = _passwordHasher.HashPassword(request.Password);

        User user;
        if (await _userRepository.DoesManagerExistAsync(cancellationToken))
        {
            user = User.CreateClient(
                            request.Email,
                            passwordHash,
                            request.Name,
                            request.Phone
                        );
        }
        else
        {
            user = User.CreateManager(request.Email, passwordHash);
        }

        var numOfAddedEntities = await _userRepository.AddAsync(user);
        if(numOfAddedEntities == 0) 
        {
            throw new HttpException(HttpStatusCode.InternalServerError, "Возникла ошибка при добавлении в базу данных");
        }
    }
}