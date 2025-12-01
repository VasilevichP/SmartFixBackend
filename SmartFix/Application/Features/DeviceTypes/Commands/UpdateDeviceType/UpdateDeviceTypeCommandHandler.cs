using MediatR;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.DeviceTypes.Commands.UpdateDeviceType;

public class UpdateDeviceTypeCommandHandler: IRequestHandler<UpdateDeviceTypeCommand>
{
    private readonly IDeviceTypeRepository _deviceTypeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDeviceTypeCommandHandler(IDeviceTypeRepository deviceTypeRepository, IUnitOfWork unitOfWork)
    {
        _deviceTypeRepository = deviceTypeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateDeviceTypeCommand request, CancellationToken cancellationToken)
    {
        var deviceType = await _deviceTypeRepository.GetByIdAsync(request.Id, cancellationToken);
        if (deviceType == null) 
            throw new Exception($"Тип устройства с ID {request.Id} не найден.");

        deviceType.Update(request.Name);
        
        _deviceTypeRepository.Update(deviceType);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
    
}