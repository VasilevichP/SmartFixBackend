using System.Net;
using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.Requests.Commands.ChangeServicesList;

public class AddServiceToRequestCommandHandler: IRequestHandler<AddServiceToRequestCommand>
{
    private readonly IRequestRepository _requestRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublisher _publisher;

    public AddServiceToRequestCommandHandler(IRequestRepository requestRepository, IServiceRepository serviceRepository, IUnitOfWork unitOfWork, IPublisher publisher)
    {
        _requestRepository = requestRepository;
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    public async Task Handle(AddServiceToRequestCommand request, CancellationToken cancellationToken)
    {
        var requestEntity = await _requestRepository.GetByIdAsync(request.RequestId, cancellationToken);
        if (requestEntity == null)
        {
            throw new HttpException(HttpStatusCode.NotFound, "Заявка не найдена.");
        }

        if (request.ServiceId.HasValue)
        {
            var service = await _serviceRepository.GetByIdAsync(request.ServiceId.Value, cancellationToken);
            if (service == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Услуга не найдена.");
            }
            requestEntity.AddService(service);
        }
        else
        {
            if (!request.ServicePrice.HasValue)
            {
                throw new HttpException(HttpStatusCode.BadRequest, "Укажите цену добавленной услуги");
            }
            requestEntity.AddService(request.ServiceName, request.ServicePrice.Value);
        }
        _requestRepository.Update(requestEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}