using System.Net;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SmartFix.Application.Authentication;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingUser != null)
        {
            throw new HttpException(HttpStatusCode.BadRequest, "Пользователь с таким email уже существует.");
        }

        var passwordHash = _passwordHasher.HashPassword(request.Password);

        var client = Client.Create(
            request.Email,
            passwordHash,
            request.Name,
            request.Phone
        );

        await _userRepository.AddAsync(client, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}