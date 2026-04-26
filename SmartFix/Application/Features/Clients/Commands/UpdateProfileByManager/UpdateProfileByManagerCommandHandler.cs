using System.Net;
using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.Clients.Commands.UpdateProfileByManager;

public class UpdateProfileByManagerCommandHandler : IRequestHandler<UpdateProfileByManagerCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProfileByManagerCommandHandler(IUserRepository repo, IUnitOfWork uow)
    {
        _userRepository = repo;
        _unitOfWork = uow;
    }

    public async Task Handle(UpdateProfileByManagerCommand request, CancellationToken ct)
    {
        var client = await _userRepository.GetClientByIdAsync(request.Id, ct);
        if (client == null) throw new HttpException(HttpStatusCode.NotFound, "Клиент не найден.");

        if (client.Email != request.Email)
        {
            var existing = await _userRepository.GetByEmailAsync(request.Email, ct);
            if (existing != null)
                throw new HttpException(HttpStatusCode.NotFound, "Этот Email уже занят другим пользователем.");
        }

        client.UpdateClient(request.Email, request.Name, request.Phone);
        client.SetPersonalDiscount(request.PersonalDiscount);
        client.SetStatus(request.Status);
        
        _userRepository.Update(client);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}