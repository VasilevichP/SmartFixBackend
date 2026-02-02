using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;

namespace SmartFix.Application.Features.Services.Commands.CreateService;

public class CreateServiceCommandHandler: IRequestHandler<CreateServiceCommand>
{
    private readonly IServiceRepository _serviceRepository;
    private IDeviceModelRepository _deviceModelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateServiceCommandHandler(IServiceRepository serviceRepository, IUnitOfWork unitOfWork, IDeviceModelRepository deviceModelRepository)
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
                throw new Exception($"Модель устройства с ID {request.DeviceModelId} не найдена.");
            }
            
            if (dbModel.DeviceTypeId != request.DeviceTypeId)
            {
                throw new Exception($"Ошибка валидации: Модель '{dbModel.Name}' относится к типу '{dbModel.DeviceType.Name}', а вы пытаетесь создать услугу для другого типа устройства.");
            }

            if (request.ManufacturerId.HasValue && dbModel.ManufacturerId != request.ManufacturerId.Value)
            {
                throw new Exception($"Ошибка валидации: Модель '{dbModel.Name}' принадлежит производителю '{dbModel.Manufacturer.Name}', а в форме выбран другой производитель.");
            }
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