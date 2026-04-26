using System.Net;
using MediatR;
using SmartFix.Application.Common.Events;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.Requests.Commands.CreateRequest;

public class CreateRequestCommandHandler : IRequestHandler<CreateRequestCommand, Guid>
{
    private readonly IRequestRepository _requestRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;
    private readonly IPublisher _publisher;

    public CreateRequestCommandHandler(IRequestRepository requestRepository, IUnitOfWork unitOfWork,
        IFileService fileService, IPublisher publisher, IServiceRepository serviceRepository)
    {
        _requestRepository = requestRepository;
        _unitOfWork = unitOfWork;
        _fileService = fileService;
        _publisher = publisher;
        _serviceRepository = serviceRepository;
    }

    public async Task<Guid> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
    {
        var domainRequest = Request.Create(
            clientId: request.ClientId,
            type:request.Type,
            deviceTypeId: request.DeviceTypeId,
            deviceModelName: request.DeviceModelName,
            description: request.Description,
            contactName: request.ContactName,
            contactPhone: request.ContactPhone,
            contactEmail: request.ContactEmail,
            serialNumber: request.SerialNumber,
            promoCodeId: request.PromoCodeId,
            fieldAddress: request.FieldAddress,
            scheduledTime: request.ScheduledTime,
            parentRequestId: request.ParentRequestId
        );
        if (request.Photos != null && request.Photos.Count > 0)
        {
            if (request.Photos.Count > 5)
                throw new HttpException(HttpStatusCode.BadRequest, "Максимальное количество фото — 5.");

            foreach (var file in request.Photos)
            {
                var savedPath = await _fileService.SaveFileAsync(file, "requests", cancellationToken);
                domainRequest.AddPhoto(file.FileName, savedPath);
            }
        }

        await _requestRepository.AddAsync(domainRequest, cancellationToken);
        
        if (request.ServiceIds.Any())
        {
            foreach (var serviceId in request.ServiceIds)
            {
                var service = await _serviceRepository.GetByIdAsync(serviceId, cancellationToken);
                if (service != null)
                {
                    domainRequest.AddService(service.Id, service.Name, service.Price);
                }
            }
        }
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _publisher.Publish(new RequestCreatedEvent(
            domainRequest.Id,
            domainRequest.ContactEmail,
            domainRequest.ContactName
        ), cancellationToken);

        return domainRequest.Id;
    }
}