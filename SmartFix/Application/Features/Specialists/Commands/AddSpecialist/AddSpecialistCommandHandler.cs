using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;

namespace SmartFix.Application.Features.Specialists.Commands.AddSpecialist;

public class CreateSpecialistCommandHandler : IRequestHandler<AddSpecialistCommand>
{
    private readonly ISpecialistRepository _specialistRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSpecialistCommandHandler(ISpecialistRepository specialistRepository, IUnitOfWork unitOfWork)
    {
        _specialistRepository = specialistRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AddSpecialistCommand request, CancellationToken cancellationToken)
    {
        var specialist = Specialist.Create(request.FullName);

        await _specialistRepository.AddAsync(specialist, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}