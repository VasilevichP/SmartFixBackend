using System.Net;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SmartFix.Application.Authentication;
using SmartFix.Application.Common.Events;
using SmartFix.Application.Helpers;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.Requests.Commands.CreateRequestAsManager;

public class CreateRequestAsManagerCommandHandler: IRequestHandler<CreateRequestAsManagerCommand, Guid>
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

    public async Task<Guid> Handle(CreateRequestAsManagerCommand request, CancellationToken ct)
    {
        Guid finalClientId;
        int clientOrdersCount = 0;
        decimal personalDiscount = 0;

        if (request.ClientId.HasValue)
        {
            var client = await _userRepository.GetClientByIdAsync(request.ClientId.Value, ct);
            if (client == null) throw new HttpException(HttpStatusCode.NotFound,"Выбранный клиент не найден.");
            
            finalClientId = client.Id;
            personalDiscount = client.PersonalDiscount;
            clientOrdersCount = await _userRepository.GetClientOrdersCountAsync(finalClientId, ct);
        }
        else
        {
            if (await _userRepository.FindByEmailAsync(request.ContactEmail, ct))
                throw new HttpException(HttpStatusCode.BadRequest,"Клиент с таким Email уже существует. Выберите его из базы.");

            var tempPassword = Guid.NewGuid().ToString().Substring(0, 8);
            var hash = _passwordHasher.HashPassword(tempPassword);

            var newClient = Client.Create(request.ContactEmail, hash, request.ContactName, request.ContactPhone);
            await _userRepository.AddAsync(newClient, ct);
            
            finalClientId = newClient.Id;
            clientOrdersCount = 0;
        }

        var domainRequest = Request.Create(
            clientId: finalClientId,
            type: request.Type,
            deviceTypeId: request.DeviceTypeId,
            deviceModelName: request.DeviceModelName,
            description: request.Description,
            contactName: request.ContactName,
            contactPhone: request.ContactPhone,
            contactEmail:request.ContactEmail,
            serialNumber: request.SerialNumber,
            null,
            fieldAddress: request.FieldAddress,
            scheduledTime: request.ScheduledTime,
            parentRequestId: request.ParentRequestId
        );
        if (!string.IsNullOrWhiteSpace(request.DeviceAppearance) || !string.IsNullOrWhiteSpace(request.DevicePackage))
        {
            domainRequest.UpdateAcceptanceInfo(request.DeviceAppearance ?? "", request.DevicePackage ?? "");
        }
        if (request.ServiceIds.Any())
        {
            foreach (var serviceId in request.ServiceIds)
            {
                var service = await _serviceRepository.GetByIdAsync(serviceId, ct);
                if (service != null)
                {
                    domainRequest.AddService(service.Id, service.Name, service.Price);
                }
            }
        }
        if (request.CustomServices.Any())
        {
            foreach (var customService in request.CustomServices)
            {
                domainRequest.AddService(null, customService.Name, customService.Price);
            }
        }
        var allActiveRules = await _discountRepository.GetAllActiveAsync(ct);
        DiscountCalculator.CalculateAndApplyDiscounts(domainRequest, allActiveRules, clientOrdersCount, personalDiscount);

        await _requestRepository.AddAsync(domainRequest, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        await _publisher.Publish(new RequestCreatedEvent(
            domainRequest.Id,
            domainRequest.ContactEmail,
            domainRequest.ContactName
        ), ct);
        
        return domainRequest.Id;
    }
}