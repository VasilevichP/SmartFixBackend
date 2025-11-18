using MediatR;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Specialists.Commands.DeleteSpecialist;

public class DeleteSpecialistCommandHandler: IRequestHandler<DeleteSpecialistCommand>
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
        var category = await _specialistRepository.GetByIdAsync(request.Id, cancellationToken);
        if (category == null) return;

        // TODO: проверка наличия заявок

        _specialistRepository.Delete(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}