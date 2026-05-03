using System.Net;
using MediatR;
using SmartFix.Application.Features.Requests.Commands.AssignMaster;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Exceptions;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Features.Requests.Commands.AssignMaster;

public class AssignMasterCommandHandler : IRequestHandler<AssignMasterCommand>
{
    private readonly IRequestRepository _requestRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AssignMasterCommandHandler(
        IRequestRepository requestRepository,
        IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        _requestRepository = requestRepository;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }

    public async Task Handle(AssignMasterCommand request, CancellationToken cancellationToken)
    {
        var requestEntity = await _requestRepository.GetByIdAsync(request.RequestId, cancellationToken);
        if (requestEntity == null) throw new HttpException(HttpStatusCode.NotFound, "Заявка не найдена");

        var master = await _userRepository.GetMasterByIdAsync(request.MasterId, cancellationToken);
        if (master == null) 
            throw new HttpException(HttpStatusCode.NotFound, "Мастер не найден.");
        
        requestEntity.AssignMaster(request.MasterId);

        _requestRepository.Update(requestEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}