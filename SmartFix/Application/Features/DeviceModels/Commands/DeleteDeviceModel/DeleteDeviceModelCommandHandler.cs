using System.Net;
using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.DeviceModels.Commands.DeleteDeviceModel;

public class DeleteDeviceModelCommandHandler: IRequestHandler<DeleteDeviceModelCommand>
{
    private readonly IDeviceModelRepository _deviceModelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDeviceModelCommandHandler(IDeviceModelRepository deviceModelRepository, IUnitOfWork unitOfWork)
    {
        _deviceModelRepository = deviceModelRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteDeviceModelCommand request, CancellationToken cancellationToken)
    {
        var deviceModel = await _deviceModelRepository.GetByIdAsync(request.Id, cancellationToken);
        if (deviceModel == null) throw new HttpException(HttpStatusCode.NotFound,"Выбранная модель устройства не найдена");

        bool isUsed = await _deviceModelRepository.IsUsedInSystemAsync(deviceModel.Id, cancellationToken);

        if (isUsed)
        {
            deviceModel.Archive(); 
            _deviceModelRepository.Update(deviceModel);
        }
        else
        {
            _deviceModelRepository.Delete(deviceModel);
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}