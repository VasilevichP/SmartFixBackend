using MediatR;
using SmartFix.Domain.Abstractions;

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
            throw new Exception($"Производитель с ID {request.Id} не найден.");

        manufacturer.Update(request.Name);
        
        _manufacturerRepository.Update(manufacturer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}