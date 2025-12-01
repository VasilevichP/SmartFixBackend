using MediatR;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Requests.Commands.ChangeRequestStatus;

public class ChangeRequestStatusCommandHandler: IRequestHandler<ChangeRequestStatusCommand>
{
    private readonly IRequestRepository _requestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeRequestStatusCommandHandler(IRequestRepository requestRepository, IUnitOfWork unitOfWork)
    {
        _requestRepository = requestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ChangeRequestStatusCommand request, CancellationToken cancellationToken)
    {
        var domainRequest = await _requestRepository.GetByIdAsync(request.RequestId, cancellationToken);
        if (domainRequest == null)
        {
            throw new Exception($"Заявка с ID {request.RequestId} не найдена.");
        }
        
        domainRequest.ChangeStatus(request.NewStatus);

        _requestRepository.Update(domainRequest);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}