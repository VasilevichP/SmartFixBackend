using MediatR;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Services.Commands.UpdateService;

public class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateServiceCommandHandler(IServiceRepository serviceRepository, IUnitOfWork unitOfWork)
    {
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        var service = await _serviceRepository.GetByIdAsync(request.Id, cancellationToken);
        if (service == null)
        {
            throw new Exception($"Услуга с ID {request.Id} не найдена.");
        }

        service.UpdateDetails(request.Name, request.Price, request.Description, request.WarrantyPeriod,request.IsAvailable);

        _serviceRepository.Update(service);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}