using System.Net;
using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.Masters.Commands.DeleteMaster;

public class DeleteMasterCommandHandler:IRequestHandler<DeleteMasterCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteMasterCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteMasterCommand request, CancellationToken cancellationToken)
    {
        var master = await _userRepository.GetMasterByIdAsync(request.Id, cancellationToken);
        if (master == null)
        {
            throw new HttpException(HttpStatusCode.NotFound, "Пользователь не найден");
        }
        
        master.Delete();
        _userRepository.Update(master, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}