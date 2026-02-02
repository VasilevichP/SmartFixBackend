using MediatR;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Manufacturers.Commands.DeleteManufacturer;

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
        if (deviceModel == null) return;

        // TODO: ВАЖНО! Перед удалением категории нужно проверить, не привязаны ли к ней какие-либо услуги.
        // if (category.Services.Any()) 
        // {
        //     throw new Exception("Невозможно удалить категорию, так как к ней привязаны услуги.");
        // }
        // Для этого нужно будет в GetByIdAsync добавить .Include(c => c.Services)

        _deviceModelRepository.Delete(deviceModel);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}