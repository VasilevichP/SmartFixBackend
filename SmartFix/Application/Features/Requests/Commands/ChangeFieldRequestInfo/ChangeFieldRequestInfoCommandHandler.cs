using System.Net;
using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.Requests.Commands.ChangeFieldRequestInfo;

public class ChangeFieldRequestInfoCommandHandler:IRequestHandler<ChangeFieldRequestInfoCommand>
{
    private readonly IRequestRepository _requestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeFieldRequestInfoCommandHandler(IRequestRepository requestRepository, IUnitOfWork unitOfWork)
    {
        _requestRepository = requestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ChangeFieldRequestInfoCommand request, CancellationToken cancellationToken)
    {
        var requestEntity = await _requestRepository.GetByIdAsync(request.Id, cancellationToken);
        if (requestEntity == null)
        {
            throw new HttpException(HttpStatusCode.NotFound, "Заявка не найдена.");
        }

        requestEntity.UpdateFieldRequestInfo(request.FieldAddress,request.ScheduledTime);

        _requestRepository.Update(requestEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}