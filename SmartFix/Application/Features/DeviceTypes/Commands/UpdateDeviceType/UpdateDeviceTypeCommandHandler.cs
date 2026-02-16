using System.Net;
using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.DeviceTypes.Commands.UpdateDeviceType;

public class UpdateDeviceTypeCommandHandler : IRequestHandler<UpdateDeviceTypeCommand>
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
            throw new HttpException(HttpStatusCode.NotFound, "Выбранный тип устройства не найден");
        if (deviceType.Name != request.Name)
        {
            if (await _deviceTypeRepository.ExistsByName(request.Name, cancellationToken))
            {
                throw new HttpException(HttpStatusCode.BadRequest, "Тип устройства с таким названием уже существует");
            }

            deviceType.Update(request.Name);

            _deviceTypeRepository.Update(deviceType);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}