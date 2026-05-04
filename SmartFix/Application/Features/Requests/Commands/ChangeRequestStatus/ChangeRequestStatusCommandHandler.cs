using System.Diagnostics;
using System.Net;
using MediatR;
using SmartFix.Application.Common.Events;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Exceptions;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Features.Requests.Commands.ChangeRequestStatus;

public class ChangeRequestStatusCommandHandler: IRequestHandler<ChangeRequestStatusCommand>
{
    private readonly IPublisher _publisher; 
    private readonly IRequestRepository _requestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeRequestStatusCommandHandler(IRequestRepository requestRepository, IUnitOfWork unitOfWork, IPublisher publisher)
    {
        _requestRepository = requestRepository;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    public async Task Handle(ChangeRequestStatusCommand request, CancellationToken cancellationToken)
    {
        var requestEntity = await _requestRepository.GetByIdAsync(request.RequestId, cancellationToken);
        if (requestEntity == null)
        {
            throw new HttpException(HttpStatusCode.NotFound, "Заявка не найдена.");
        }
        
        requestEntity.ChangeStatus(request.NewStatus);

        _requestRepository.Update(requestEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (request.NewStatus == RequestStatus.Ready)
        {
            await _publisher.Publish(new RequestReadyEvent(
                requestEntity.Id,
                requestEntity.ContactEmail,
                requestEntity.ContactName
            ), cancellationToken);
        }
    }
}