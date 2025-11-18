using MediatR;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Services.Commands.DeleteService;

public class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteServiceCommandHandler(IServiceRepository serviceRepository, IUnitOfWork unitOfWork)
    {
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
    {
        var service = await _serviceRepository.GetByIdAsync(request.Id, cancellationToken);
        if (service == null)
        {
            return;
        }

        // TODO: В соответствии с User Story (US04, US05), здесь нужно добавить проверку
        // на наличие активных заявок для этой услуги перед удалением.
        // var activeRequests = _requestRepository.GetActiveByServiceId(request.Id);
        // if (activeRequests.Any()) throw new Exception("Невозможно удалить услугу...");

        _serviceRepository.Delete(service);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}