using MediatR;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Services.Commands.UpdateService;

public class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IDeviceModelRepository _deviceModelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateServiceCommandHandler(IServiceRepository serviceRepository, IUnitOfWork unitOfWork, IDeviceModelRepository deviceModelRepository)
    {
        _serviceRepository = serviceRepository;
        _deviceModelRepository = deviceModelRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        var service = await _serviceRepository.GetByIdAsync(request.Id, cancellationToken);
        if (service == null)
        {
            throw new Exception($"Услуга с ID {request.Id} не найдена.");
        }
        
        if (request.DeviceModelId.HasValue)
        {
            if (service.DeviceModelId != request.DeviceModelId)
            {
                var dbModel = await _deviceModelRepository.GetByIdAsync(request.DeviceModelId.Value, cancellationToken);
                if (dbModel == null) throw new Exception("Модель не найдена");

                if (dbModel.DeviceTypeId != request.DeviceTypeId)
                    throw new Exception($"Модель '{dbModel.Name}' не соответствует выбранному типу устройства.");

                if (request.ManufacturerId.HasValue && dbModel.ManufacturerId != request.ManufacturerId.Value)
                    throw new Exception($"Модель '{dbModel.Name}' не принадлежит выбранному производителю.");
            }
        }

        service.UpdateDetails(request.Name, request.Price, request.Description, request.WarrantyPeriod,
            request.IsAvailable, request.CategoryId, request.DeviceTypeId, request.ManufacturerId, request.DeviceModelId);

        _serviceRepository.Update(service);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}