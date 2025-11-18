using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;

namespace SmartFix.Application.Features.Services.Commands.CreateService;

public class CreateServiceCommandHandler: IRequestHandler<CreateServiceCommand>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateServiceCommandHandler(IServiceRepository serviceRepository, IUnitOfWork unitOfWork)
    {
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        var service = Service.Create(
            request.Name,
            request.Price,
            request.CategoryId,
            request.Description,
            request.WarrantyPeriod
        );

        await _serviceRepository.AddAsync(service, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}