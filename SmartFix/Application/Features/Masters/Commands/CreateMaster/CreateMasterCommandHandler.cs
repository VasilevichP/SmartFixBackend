using System.Net;
using MediatR;
using SmartFix.Application.Authentication;
using SmartFix.Application.Common.Events;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.Masters.Commands.CreateMaster;

public class CreateMasterCommandHandler:IRequestHandler<CreateMasterCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IPublisher _publisher;

    public CreateMasterCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IPublisher publisher)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _publisher = publisher;
    }

    public async Task Handle(CreateMasterCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.FindByEmailAsync(request.Email, cancellationToken))
            throw new HttpException(HttpStatusCode.BadRequest,"Пользователь с таким Email уже существует.");

        var password = Guid.NewGuid().ToString().Substring(0, 8); 
        var hash = _passwordHasher.HashPassword(password);

        var master = Master.Create(request.Email, hash, request.Name, request.PhoneNumber);
       
        await _userRepository.AddAsync(master, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _publisher.Publish(new UserCreatedEvent(
            request.Email,
            password
        ), cancellationToken);
    }
}