using MediatR;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Specialists.Commands.DeleteSpecialist;

public class DeleteSpecialistCommandHandler : IRequestHandler<DeleteSpecialistCommand>
{
    private readonly ISpecialistRepository _specialistRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSpecialistCommandHandler(ISpecialistRepository specialistRepository, IUnitOfWork unitOfWork)
    {
        _specialistRepository = specialistRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteSpecialistCommand request, CancellationToken cancellationToken)
    {
        var specialist = await _specialistRepository.GetByIdAsync(request.Id, cancellationToken);
        if (specialist == null) throw new Exception("Выбранный специалист не найден");
        
        if (await _specialistRepository.HasRelatedRequestsAsync(specialist.Id, cancellationToken))
            throw new InvalidOperationException("Нельзя удалить Специалиста: сперва переназначте заявки.");

        _specialistRepository.Delete(specialist);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}