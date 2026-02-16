using System.Net;
using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.Specialists.Commands.UpdateSpecialist;

public class UpdateSpecialistCommandHandler : IRequestHandler<UpdateSpecialistCommand>
{
    private readonly ISpecialistRepository _specialistRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSpecialistCommandHandler(ISpecialistRepository specialistRepository, IUnitOfWork unitOfWork)
    {
        _specialistRepository = specialistRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateSpecialistCommand request, CancellationToken cancellationToken)
    {
        var specialist = await _specialistRepository.GetByIdAsync(request.Id, cancellationToken);
        if (specialist == null)
            throw new HttpException(HttpStatusCode.NotFound, "Выбранный специалист не найден");
        if (specialist.Name != request.Name)
        {
            if (await _specialistRepository.ExistsByName(request.Name, cancellationToken))
            {
                throw new HttpException(HttpStatusCode.BadRequest, "Специалист с таким именем уже существует");
            }

            specialist.Update(request.Name);

            _specialistRepository.Update(specialist);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}