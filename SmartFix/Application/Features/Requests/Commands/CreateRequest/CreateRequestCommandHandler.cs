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
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;
    private readonly IPublisher _publisher;

    public CreateRequestCommandHandler(IRequestRepository requestRepository, IUnitOfWork unitOfWork,
        IFileService fileService, IPublisher publisher)
    {
        _requestRepository = requestRepository;
        _unitOfWork = unitOfWork;
        _fileService = fileService;
        _publisher = publisher;
    }

    public async Task<Guid> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
    {
        var domainRequest = Request.Create(
            clientId: request.ClientId,
            contactEmail: request.ContactEmail,
            contactPhone: request.ContactPhoneNumber,
            contactName: request.ContactName,
            deviceTypeId: request.DeviceTypeId,
            description: request.Description,
            price: request.Price,
            serviceId: request.ServiceId,
            deviceModelId: request.DeviceModelId,
            deviceModelName: request.DeviceModelName,
            deviceSerialNumber: request.DeviceSerialNumber
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
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _publisher.Publish(new RequestCreatedEvent(
            domainRequest.Id,
            domainRequest.ContactEmail,
            domainRequest.ContactName
        ), cancellationToken);

        return domainRequest.Id;
    }
}