using System.Net;
using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.Masters.Commands.UpdateMaster;

public class UpdateMasterCommandHandler:IRequestHandler<UpdateMasterCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMasterCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateMasterCommand request, CancellationToken cancellationToken)
    {
        var master = await _userRepository.GetMasterByIdAsync(request.Id, cancellationToken);
        if (master == null)
        {
            throw new HttpException(HttpStatusCode.NotFound, "Пользователь не найден");
        }
        if (!master.Email.Equals(request.Email) && _userRepository.FindByEmailAsync(request.Email,cancellationToken).Result)
        {
            throw new HttpException(HttpStatusCode.BadRequest, "Пользователь с таким email уже существует.");
        }

        master.Update(request.Email, request.Name, request.PhoneNumber);
        _userRepository.Update(master, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}