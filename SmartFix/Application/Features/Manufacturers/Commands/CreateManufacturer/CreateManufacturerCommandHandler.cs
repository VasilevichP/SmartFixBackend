using System.Net;
using MediatR;
using SmartFix.Application.Features.ServiceCategories.Commands.CreateCategory;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.Manufacturers.Commands.CreateManufacturer;

public class CreateManufacturerCommandHandler: IRequestHandler<CreateManufacturerCommand>
{
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateManufacturerCommandHandler(IManufacturerRepository manufacturerRepository, IUnitOfWork unitOfWork)
    {
        _manufacturerRepository = manufacturerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateManufacturerCommand request, CancellationToken cancellationToken)
    {
        if (await _manufacturerRepository.ExistsByName(request.Name, cancellationToken))
        {
            throw new HttpException(HttpStatusCode.BadRequest,"Производитель с таким названием уже существует");
        }
        var manufacturer = Manufacturer.Create(request.Name);

        await _manufacturerRepository.AddAsync(manufacturer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
