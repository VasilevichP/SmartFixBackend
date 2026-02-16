using System.Net;
using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Domain.Exceptions;

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
        if (await _specialistRepository.ExistsByName(request.Name, cancellationToken))
        {
            throw new HttpException(HttpStatusCode.BadRequest, "Специалист с таким именем уже существует");
        }
        var specialist = Specialist.Create(request.Name);

        await _specialistRepository.AddAsync(specialist, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}