using MediatR;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Manufacturers.Commands.UpdateManufacturer;

public class UpdateDeviceModelCommandHandler: IRequestHandler<UpdateDeviceModelCommand>
{
    private readonly IDeviceModelRepository _deviceModelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDeviceModelCommandHandler(IDeviceModelRepository deviceModelRepository, IUnitOfWork unitOfWork)
    {
        _deviceModelRepository = deviceModelRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateDeviceModelCommand request, CancellationToken cancellationToken)
    {
        var deviceModel = await _deviceModelRepository.GetByIdAsync(request.Id, cancellationToken);
        if (deviceModel == null) 
            throw new Exception($"Производитель с ID {request.Id} не найден.");

        deviceModel.Update(request.Name, request.ManufacturerId, request.DeviceTypeId);
        
        _deviceModelRepository.Update(deviceModel);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}