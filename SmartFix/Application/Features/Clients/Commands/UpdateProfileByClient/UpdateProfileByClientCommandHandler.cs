using System.Net;
using MediatR;
using SmartFix.Application.Features.Clients.Commands.UpdateProfileByClient;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.Users.Commands.UpdateProfileByClient;

public class UpdateProfileByClientCommandHandler: IRequestHandler<UpdateProfileByClientCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProfileByClientCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateProfileByClientCommand request, CancellationToken cancellationToken)
    {
        var client = await _userRepository.GetClientByIdAsync(request.Id, cancellationToken);
        if (client == null)
        {
            throw new HttpException(HttpStatusCode.NotFound, "Пользователь не найден");
        }
        if (!client.Email.Equals(request.Email) && _userRepository.FindByEmailAsync(request.Email,cancellationToken).Result)
        {
            throw new HttpException(HttpStatusCode.BadRequest, "Пользователь с таким email уже существует.");
        }

        client.UpdateClient(request.Email, request.Name, request.Phone);
        _userRepository.Update(client, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}