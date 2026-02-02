using MediatR;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Manufacturers.Commands.DeleteManufacturer;

public class DeleteManufacturerCommandHandler: IRequestHandler<DeleteManufacturerCommand>
{
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteManufacturerCommandHandler(IManufacturerRepository manufacturerRepository, IUnitOfWork unitOfWork)
    {
        _manufacturerRepository = manufacturerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteManufacturerCommand request, CancellationToken cancellationToken)
    {
        var manufacturer = await _manufacturerRepository.GetByIdAsync(request.Id, cancellationToken);
        if (manufacturer == null) return;

        // TODO: ВАЖНО! Перед удалением категории нужно проверить, не привязаны ли к ней какие-либо услуги.
        // if (category.Services.Any()) 
        // {
        //     throw new Exception("Невозможно удалить категорию, так как к ней привязаны услуги.");
        // }
        // Для этого нужно будет в GetByIdAsync добавить .Include(c => c.Services)

        _manufacturerRepository.Delete(manufacturer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}