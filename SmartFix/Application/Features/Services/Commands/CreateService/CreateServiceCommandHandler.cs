using System.Net;
using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.Services.Commands.CreateService;

public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand>
{
    private readonly IServiceRepository _serviceRepository;
    private IDeviceModelRepository _deviceModelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateServiceCommandHandler(IServiceRepository serviceRepository, IUnitOfWork unitOfWork,
        IDeviceModelRepository deviceModelRepository)
    {
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
        _deviceModelRepository = deviceModelRepository;
    }

    public async Task Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        if (request.DeviceModelId.HasValue)
        {
            var dbModel = await _deviceModelRepository.GetByIdAsync(request.DeviceModelId.Value, cancellationToken);

            if (dbModel == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, $"Модель устройства {request.Name} не найдена.");
            }

            if (dbModel.DeviceTypeId != request.DeviceTypeId)
            {
                throw new HttpException(HttpStatusCode.BadRequest,
                    $"Модель '{dbModel.Name}' не соответствует выбранному типу устройства.");
            }

            if (request.ManufacturerId.HasValue && dbModel.ManufacturerId != request.ManufacturerId.Value)
            {
                throw new HttpException(HttpStatusCode.BadRequest,
                    $"Модель '{dbModel.Name}' не принадлежит выбранному производителю.");
            }
        }

        bool isDuplicate = await _serviceRepository.IsDuplicateAsync(
            request.Name,
            request.DeviceTypeId,
            request.DeviceModelId,
            cancellationToken);

        if (isDuplicate)
        {
            throw new HttpException(HttpStatusCode.BadRequest,
                "Услуга с таким названием для данного устройства уже существует.");
        }


        var service = Service.Create(
            request.Name,
            request.Price,
            request.CategoryId,
            request.Description,
            request.WarrantyPeriod,
            request.DeviceTypeId,
            request.DeviceModelId,
            request.ManufacturerId
        );

        await _serviceRepository.AddAsync(service, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}