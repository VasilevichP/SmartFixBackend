using System.Net;
using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.Manufacturers.Commands.UpdateManufacturer;

public class UpdateManufacturerCommandHandler: IRequestHandler<UpdateManufacturerCommand>
{
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateManufacturerCommandHandler(IManufacturerRepository manufacturerRepository, IUnitOfWork unitOfWork)
    {
        _manufacturerRepository = manufacturerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateManufacturerCommand request, CancellationToken cancellationToken)
    {
        var manufacturer = await _manufacturerRepository.GetByIdAsync(request.Id, cancellationToken);
        if (manufacturer == null)
            throw new HttpException(HttpStatusCode.NotFound,"Выбранный производитель не найден");
        if (manufacturer.Name != request.Name)
        {
            if (await _manufacturerRepository.ExistsByName(request.Name, cancellationToken))
            {
                throw new HttpException(HttpStatusCode.BadRequest,"Производитель с таким названием уже существует");
            }

            manufacturer.Update(request.Name);

            _manufacturerRepository.Update(manufacturer);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}