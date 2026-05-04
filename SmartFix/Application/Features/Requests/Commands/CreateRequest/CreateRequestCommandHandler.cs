using System.Net;
using MediatR;
using SmartFix.Application.Common.Events;
using SmartFix.Application.Helpers;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Domain.Exceptions;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Features.Requests.Commands.CreateRequest;

public class CreateRequestCommandHandler : IRequestHandler<CreateRequestCommand, Guid>
{
    private readonly IRequestRepository _requestRepository;
    private readonly IUserRepository _userRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IDiscountRepository _discountRepository;
    private readonly IPromoCodeRepository _promoCodeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;
    private readonly IPublisher _publisher;

    public CreateRequestCommandHandler(IRequestRepository requestRepository, IUnitOfWork unitOfWork,
        IFileService fileService, IPublisher publisher, IServiceRepository serviceRepository, IDiscountRepository discountRepository, IPromoCodeRepository promoCodeRepository, IUserRepository userRepository)
    {
        _requestRepository = requestRepository;
        _unitOfWork = unitOfWork;
        _fileService = fileService;
        _publisher = publisher;
        _serviceRepository = serviceRepository;
        _discountRepository = discountRepository;
        _promoCodeRepository = promoCodeRepository;
        _userRepository = userRepository;
    }

    public async Task<Guid> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
    {
        var client = await _userRepository.GetClientByIdAsync(request.ClientId, cancellationToken);
        if (client == null) throw new HttpException(HttpStatusCode.NotFound,"Клиент не найден");

        int clientOrdersCount = await _userRepository.GetClientOrdersCountAsync(client.Id, cancellationToken);

        var domainRequest = Request.Create(
            clientId: client.Id,
            type: request.Type,
            deviceTypeId: request.DeviceTypeId,
            deviceModelId:request.DeviceModelId,
            deviceModelName: request.DeviceModelName,
            description: request.Description,
            contactName: request.ContactName,
            contactPhone: request.ContactPhone,
            contactEmail: request.ContactEmail,
            serialNumber: request.SerialNumber,
            promoCodeId:null,
            fieldAddress: request.FieldAddress,
            scheduledTime: request.ScheduledTime,
            parentRequestId: null
        );

        if (request.ServiceIds != null && request.ServiceIds.Any())
        {
            foreach (var serviceId in request.ServiceIds)
            {
                var service = await _serviceRepository.GetByIdAsync(serviceId, cancellationToken);
                if (service != null)
                {
                    domainRequest.AddService(service.Id, service.Name, service.Price, service.WarrantyPeriod);
                }
            }
        }
        PromoCode? validPromo = null;
        if (!string.IsNullOrWhiteSpace(request.PromoCodeCode))
        {
            validPromo = await _promoCodeRepository.GetByCodeAsync(request.PromoCodeCode.Trim(), cancellationToken);
            if (validPromo == null || !validPromo.IsValid())
                throw new HttpException(HttpStatusCode.BadRequest,"Введенный промокод недействителен или его срок истек");

            validPromo.DecrementLimit();
            _promoCodeRepository.Update(validPromo);
            domainRequest.PromoCodeId = validPromo.Id;
        }
        var activeDiscounts = await _discountRepository.GetAllActiveAsync(cancellationToken);
        DiscountCalculator.CalculateAndApplyDiscounts(domainRequest, activeDiscounts, clientOrdersCount, client.PersonalDiscount, validPromo);
        
        if (request.Photos != null && request.Photos.Any())
        {
            if (request.Photos.Count > 5)
                throw new HttpException(HttpStatusCode.BadRequest,"Максимальное количество фото — 5");

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