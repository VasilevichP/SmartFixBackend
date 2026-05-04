using System.Net;
using MediatR;
using SmartFix.Application.Authentication;
using SmartFix.Application.Common.Events;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.Clients.Commands.CreateProfileByManager;

public class CreateProfileByManagerCommandHandler: IRequestHandler<CreateProfileByManagerCommand>
{
private readonly IUserRepository _userRepository;
private readonly IUnitOfWork _unitOfWork;
private readonly IPasswordHasher _passwordHasher;
private readonly IPublisher _publisher; 

public CreateProfileByManagerCommandHandler(IUserRepository repo, IUnitOfWork uow, IPasswordHasher hasher, IPublisher publisher)
{
    _userRepository = repo;
    _unitOfWork = uow;
    _passwordHasher = hasher;
    _publisher = publisher;
}

public async Task Handle(CreateProfileByManagerCommand request, CancellationToken cancellationToken)
{
    if (await _userRepository.FindByEmailAsync(request.Email, cancellationToken))
        throw new HttpException(HttpStatusCode.BadRequest,"Пользователь с таким Email уже существует.");

    var password = Guid.NewGuid().ToString().Substring(0, 8); 
    var hash = _passwordHasher.HashPassword(password);

    var client = Client.Create(request.Email, hash, request.Name, request.Phone);
    if (request.PersonalDiscount > 0)
    {
        client.SetPersonalDiscount(request.PersonalDiscount);
    }

    await _userRepository.AddAsync(client, cancellationToken);
    await _unitOfWork.SaveChangesAsync(cancellationToken);
    await _publisher.Publish(new UserCreatedEvent(
        request.Email,
        password
    ), cancellationToken);
}
}