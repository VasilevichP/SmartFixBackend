using System.Net;
using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Exceptions;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Features.Requests.Commands.AssignSpecialist;

public class AssignSpecialistCommandHandler : IRequestHandler<AssignSpecialistCommand>
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
        var requestEntity = await _requestRepository.GetByIdAsync(request.RequestId, cancellationToken);
        if (requestEntity == null) throw new HttpException(HttpStatusCode.NotFound, "Заявка не найдена");

        var specialist = await _specialistRepository.GetByIdAsync(request.SpecialistId, cancellationToken);
        if (specialist == null) throw new HttpException(HttpStatusCode.NotFound, "Специалист не найден");

        requestEntity.AssignSpecialist(request.SpecialistId);

        _requestRepository.Update(requestEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}