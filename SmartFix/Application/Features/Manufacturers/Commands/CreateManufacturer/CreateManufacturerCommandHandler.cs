using MediatR;
using SmartFix.Application.Features.ServiceCategories.Commands.CreateCategory;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;

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
        var manufacturer = Manufacturer.Create(request.Name);

        await _manufacturerRepository.AddAsync(manufacturer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
