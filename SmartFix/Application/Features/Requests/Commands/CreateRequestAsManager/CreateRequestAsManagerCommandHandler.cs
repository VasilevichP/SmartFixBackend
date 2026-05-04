using System.Net;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SmartFix.Application.Authentication;
using SmartFix.Application.Common.Events;
using SmartFix.Application.Helpers;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Domain.Exceptions;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Features.Requests.Commands.CreateRequestAsManager;

public class CreateRequestAsManagerCommandHandler : IRequestHandler<CreateRequestAsManagerCommand, Guid>
{
    private readonly IRequestRepository _requestRepository;
    private readonly IUserRepository _userRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IDiscountRepository _discountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IPublisher _publisher;

    public CreateRequestAsManagerCommandHandler(
        IRequestRepository requestRepository,
        IUserRepository userRepository,
        IServiceRepository serviceRepository,
        IDiscountRepository discountRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher, IPublisher publisher)
    {
        _requestRepository = requestRepository;
        _userRepository = userRepository;
        _serviceRepository = serviceRepository;
        _discountRepository = discountRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _publisher = publisher;
    }

    public async Task<Guid> Handle(CreateRequestAsManagerCommand request, CancellationToken cancellationToken)
    {
        Guid finalClientId;
        int clientOrdersCount = 0;
        decimal personalDiscount = 0;

        if (request.ClientId.HasValue)
        {
            var client = await _userRepository.GetClientByIdAsync(request.ClientId.Value, cancellationToken);
            if (client == null) throw new HttpException(HttpStatusCode.NotFound, "Выбранный клиент не найден.");

            finalClientId = client.Id;
            personalDiscount = client.PersonalDiscount;
            clientOrdersCount = await _userRepository.GetClientOrdersCountAsync(finalClientId, cancellationToken);
        }
        else
        {
            if (await _userRepository.FindByEmailAsync(request.ContactEmail, cancellationToken))
                throw new HttpException(HttpStatusCode.BadRequest,
                    "Клиент с таким Email уже существует. Выберите его из базы.");

            var tempPassword = Guid.NewGuid().ToString().Substring(0, 8);
            var hash = _passwordHasher.HashPassword(tempPassword);

            var newClient = Client.Create(request.ContactEmail, hash, request.ContactName, request.ContactPhone);
            await _userRepository.AddAsync(newClient, cancellationToken);

            finalClientId = newClient.Id;
            clientOrdersCount = 0;
        }

        Guid? deviceTypeId = request.DeviceTypeId;
        Guid? deviceModelId = request.DeviceModelId;
        string deviceModelName = request.DeviceModelName;
        string serialNumber = request.SerialNumber;
        Request? parentRequest = null;

        if (request.Type == RequestType.Warranty)
        {
            if (!request.ParentRequestId.HasValue)
                throw new HttpException(HttpStatusCode.BadRequest, "Не указана родительская заявка");

            parentRequest = await _requestRepository.GetByIdAsync(request.ParentRequestId.Value, cancellationToken);
            if (parentRequest == null || parentRequest.Status != RequestStatus.Closed)
                throw new HttpException(HttpStatusCode.BadRequest, "Родительская заявка не найдена или не закрыта");

            bool isUnderWarranty = parentRequest.Services.Any(s =>
                s.WarrantyEndDate.HasValue && s.WarrantyEndDate.Value >= DateTime.UtcNow);
            if (!isUnderWarranty)
                throw new HttpException(HttpStatusCode.BadRequest, "Гарантийный срок по данной заявке уже истек");

            deviceTypeId = parentRequest.DeviceTypeId;
            deviceModelId = parentRequest.DeviceModelId;
            deviceModelName = parentRequest.DeviceModelName;
            serialNumber = parentRequest.DeviceSerialNumber;
        }

        var domainRequest = Request.Create(
            clientId: finalClientId,
            type: request.Type,
            deviceTypeId: deviceTypeId.Value,
            deviceModelId: deviceModelId,
            deviceModelName: deviceModelName,
            description: request.Description,
            contactName: request.ContactName,
            contactPhone: request.ContactPhone,
            contactEmail: request.ContactEmail,
            serialNumber: serialNumber,
            null,
            fieldAddress: request.FieldAddress,
            scheduledTime: request.ScheduledTime,
            parentRequestId: request.ParentRequestId
        );
        if (!string.IsNullOrWhiteSpace(request.DeviceAppearance) || !string.IsNullOrWhiteSpace(request.DevicePackage))
        {
            domainRequest.UpdateAcceptanceInfo(request.DeviceAppearance ?? "", request.DevicePackage ?? "");
        }

        if (request.Type == RequestType.Warranty)
        {
            foreach (var parentService in parentRequest!.Services)
            {
                domainRequest.AddService(parentService.ServiceId, parentService.ServiceName, 0, null);
            }
        }
        else
        {
            if (request.ServiceIds.Any())
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

            if (request.CustomServices.Any())
            {
                foreach (var customService in request.CustomServices)
                {
                    domainRequest.AddService(null, customService.Name, customService.Price, null);
                }
            }
        }

        var allActiveRules = await _discountRepository.GetAllActiveAsync(cancellationToken);
        DiscountCalculator.CalculateAndApplyDiscounts(domainRequest, allActiveRules, clientOrdersCount,
            personalDiscount);

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