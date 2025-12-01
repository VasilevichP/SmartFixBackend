using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;

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
        var deviceType = DeviceType.Create(request.Name);

        await _deviceTypeRepository.AddAsync(deviceType, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}