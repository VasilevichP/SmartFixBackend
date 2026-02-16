using System.Net;
using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.DeviceTypes.Commands.DeleteDeviceType;

public class DeleteDeviceTypeCommandHandler : IRequestHandler<DeleteDeviceTypeCommand>
{
    private readonly IDeviceTypeRepository _deviceTypeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDeviceTypeCommandHandler(IDeviceTypeRepository deviceTypeRepository, IUnitOfWork unitOfWork)
    {
        _deviceTypeRepository = deviceTypeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteDeviceTypeCommand request, CancellationToken cancellationToken)
    {
        var deviceType = await _deviceTypeRepository.GetByIdAsync(request.Id, cancellationToken);
        if (deviceType == null) throw new HttpException(HttpStatusCode.NotFound, "Выбранный тип устройства не найден");
        if (await _deviceTypeRepository.HasRelatedModelsAsync(deviceType.Id, cancellationToken))
            throw new HttpException(HttpStatusCode.BadRequest, "Нельзя удалить Тип: к нему привязаны модели.");

        if (await _deviceTypeRepository.HasRelatedServicesAsync(deviceType.Id, cancellationToken))
            throw new HttpException(HttpStatusCode.BadRequest, "Нельзя удалить Тип: к нему привязаны услуги.");

        if (await _deviceTypeRepository.HasRelatedRequestsAsync(deviceType.Id, cancellationToken))
            throw new HttpException(HttpStatusCode.BadRequest, "Нельзя удалить Тип: существуют заявки с этим типом.");
        _deviceTypeRepository.Delete(deviceType);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}