using System.Net;
using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.DeviceModels.Commands.UpdateDeviceModel;

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
            throw new HttpException(HttpStatusCode.NotFound,"Выбранная модель устройства не найдена");

        deviceModel.Update(request.Name, request.ManufacturerId, request.DeviceTypeId);
        
        _deviceModelRepository.Update(deviceModel);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}