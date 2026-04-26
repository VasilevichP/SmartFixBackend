using System.Net;
using MediatR;
using SmartFix.Application.Authentication;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.Users.Commands.CreateManager;

public class CreateManagerCommandHandler:IRequestHandler<CreateManagerCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public CreateManagerCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateManagerCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingUser != null)
        {
            throw new HttpException(HttpStatusCode.BadRequest, "Пользователь с таким email уже существует.");
        }

        var passwordHash = _passwordHasher.HashPassword(request.Password);

        var manager = Manager.Create(
            request.Email,
            passwordHash,
            request.Name
        );

        await _userRepository.AddAsync(manager, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}