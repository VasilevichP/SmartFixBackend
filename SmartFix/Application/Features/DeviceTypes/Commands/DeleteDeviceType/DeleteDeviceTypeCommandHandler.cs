using MediatR;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.DeviceTypes.Commands.DeleteDeviceType;

public class DeleteDeviceTypeCommandHandler: IRequestHandler<DeleteDeviceTypeCommand>
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
        if (deviceType == null) return;

        _deviceTypeRepository.Delete(deviceType);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}