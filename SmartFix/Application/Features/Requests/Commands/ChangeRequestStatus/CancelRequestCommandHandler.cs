using System.Net;
using MediatR;
using SmartFix.Application.EventHandlers;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.Requests.Commands.ChangeRequestStatus;

public class CancelRequestCommandHandler: IRequestHandler<CancelRequestCommand>
{
    private readonly IRequestRepository _requestRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublisher _publisher; 

    public CancelRequestCommandHandler(IRequestRepository requestRepository, IUnitOfWork unitOfWork, IPublisher publisher)
    {
        _requestRepository = requestRepository;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    public async Task Handle(CancelRequestCommand request, CancellationToken cancellationToken)
    {
        var requestEntity = await _requestRepository.GetByIdAsync(request.Id, cancellationToken);
        if (requestEntity == null)
        {
            throw new HttpException(HttpStatusCode.NotFound, "Заявка не найдена.");
        }
        requestEntity.Cancel(request.Reason);
        _requestRepository.Update(requestEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        await _publisher.Publish(new RequestCancelledEvent(
            requestEntity.Id,
            requestEntity.ContactEmail,
            requestEntity.ContactName
        ), cancellationToken);
    }
}