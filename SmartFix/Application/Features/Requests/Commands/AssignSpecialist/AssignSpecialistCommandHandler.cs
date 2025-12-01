using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Features.Requests.Commands.AssignSpecialist;

public class AssignSpecialistCommandHandler: IRequestHandler<AssignSpecialistCommand>
{
    private readonly IRequestRepository _requestRepository;
    private readonly ISpecialistRepository _specialistRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AssignSpecialistCommandHandler(
        IRequestRepository requestRepository,
        ISpecialistRepository specialistRepository,
        IUnitOfWork unitOfWork)
    {
        _requestRepository = requestRepository;
        _specialistRepository = specialistRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AssignSpecialistCommand request, CancellationToken cancellationToken)
    {
        var domainRequest = await _requestRepository.GetByIdAsync(request.RequestId, cancellationToken);
        if (domainRequest == null) throw new Exception("Заявка не найдена");

        var specialist = await _specialistRepository.GetByIdAsync(request.SpecialistId, cancellationToken);
        if (specialist == null) throw new Exception("Специалист не найден");

        domainRequest.AssignSpecialist(request.SpecialistId);
        domainRequest.ChangeStatus(RequestStatus.Diagnostics);

        _requestRepository.Update(domainRequest);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}