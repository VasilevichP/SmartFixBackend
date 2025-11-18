using MediatR;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Services.Commands.ChangeVisibility;

public class ChangeVisibilityCommandHandler : IRequestHandler<ToggleServiceVisibilityCommand>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeVisibilityCommandHandler(IServiceRepository serviceRepository, IUnitOfWork unitOfWork)
    {
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ToggleServiceVisibilityCommand request, CancellationToken cancellationToken)
    {
        var service = await _serviceRepository.GetByIdAsync(request.Id, cancellationToken);
        if (service == null)
        {
            throw new Exception($"Услуга с ID {request.Id} не найдена.");
        }

        if (service.IsAvailable)
        {
            service.Hide();
        }
        else
        {
            service.Show();
        }
        
        _serviceRepository.Update(service);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}