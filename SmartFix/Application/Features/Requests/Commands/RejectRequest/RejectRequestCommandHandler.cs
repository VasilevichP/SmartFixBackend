using System.Net;
using MediatR;
using SmartFix.Application.EventHandlers;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.Requests.Commands.RejectRequest;

public class RejectRequestCommandHandler: IRequestHandler<RejectRequestCommand>
{
    private readonly IRequestRepository _requestRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublisher _publisher; 
    
    public RejectRequestCommandHandler(IRequestRepository r, IUnitOfWork u, IPublisher publisher) { _requestRepository = r; _unitOfWork = u;
        _publisher = publisher;
    }

    public async Task Handle(RejectRequestCommand request, CancellationToken cancellationToken)
    {
        var requestEntity = await _requestRepository.GetByIdAsync(request.RequestId, cancellationToken);
        if (requestEntity == null) throw new HttpException(HttpStatusCode.NotFound,"Заявка не найдена");

        requestEntity.Cancel("Отказ клиента от новых условий стоимости/работ");

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _publisher.Publish(new RequestCancelledEvent(
            requestEntity.Id,
            requestEntity.ContactEmail,
            requestEntity.ContactName
        ), cancellationToken);
    }
}