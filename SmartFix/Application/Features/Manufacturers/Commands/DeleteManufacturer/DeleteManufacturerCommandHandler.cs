using System.Net;
using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.Manufacturers.Commands.DeleteManufacturer;

public class DeleteManufacturerCommandHandler : IRequestHandler<DeleteManufacturerCommand>
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
        if (manufacturer == null) throw new HttpException(HttpStatusCode.NotFound, "Выбранный производитель не найден");

        if (await _manufacturerRepository.HasRelatedModelsAsync(manufacturer.Id, cancellationToken))
            throw new HttpException(HttpStatusCode.BadRequest,
                "Нельзя удалить Производителя: к нему привязаны модели.");

        if (await _manufacturerRepository.HasRelatedServicesAsync(manufacturer.Id, cancellationToken))
            throw new HttpException(HttpStatusCode.BadRequest,
                "Нельзя удалить Производителя: к нему привязаны услуги.");

        _manufacturerRepository.Delete(manufacturer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}