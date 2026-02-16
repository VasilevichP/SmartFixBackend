using System.Net;
using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.DeviceModels.Commands.CreateDeviceModel;

public class CreateDeviceModelCommandHandler: IRequestHandler<CreateDeviceModelCommand>
{
    private readonly IDeviceModelRepository _deviceModelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDeviceModelCommandHandler(IDeviceModelRepository deviceModelRepository, IUnitOfWork unitOfWork)
    {
        _deviceModelRepository = deviceModelRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateDeviceModelCommand request, CancellationToken cancellationToken)
    {
        if (await _deviceModelRepository.ExistsByNameAndManufacturer(request.Name, request.ManufacturerId, cancellationToken))
        {
            throw new HttpException(HttpStatusCode.BadRequest,"Модель устройства с таким названием и производителем уже существует");
        }
        var deviceModel = DeviceModel.Create(request.Name, request.ManufacturerId, request.DeviceTypeId);

        await _deviceModelRepository.AddAsync(deviceModel, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
