using System.Net;
using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.DeviceTypes.Commands.AddDeviceType;

public class AddDeviceTypeCommandHandler: IRequestHandler<AddDeviceTypeCommand>
{
    private readonly IDeviceTypeRepository _deviceTypeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddDeviceTypeCommandHandler(IDeviceTypeRepository deviceTypeRepository, IUnitOfWork unitOfWork)
    {
        _deviceTypeRepository = deviceTypeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AddDeviceTypeCommand request, CancellationToken cancellationToken)
    {
        if (await _deviceTypeRepository.ExistsByName(request.Name, cancellationToken))
        {
            throw new HttpException(HttpStatusCode.BadRequest,"Тип устройства с таким названием уже существует");
        }
        var deviceType = DeviceType.Create(request.Name);

        await _deviceTypeRepository.AddAsync(deviceType, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}