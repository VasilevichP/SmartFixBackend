using System.Net;
using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Exceptions;

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
            throw new HttpException(HttpStatusCode.NotFound,"Услуга не найдена");
        }

        _serviceRepository.Delete(service);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}