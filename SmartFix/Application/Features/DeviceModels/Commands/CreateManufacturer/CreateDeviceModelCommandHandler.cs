using MediatR;
using SmartFix.Application.Features.ServiceCategories.Commands.CreateCategory;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;

namespace SmartFix.Application.Features.Manufacturers.Commands.CreateManufacturer;

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
        var deviceModel = DeviceModel.Create(request.Name, request.ManufacturerId, request.DeviceTypeId);

        await _deviceModelRepository.AddAsync(deviceModel, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
