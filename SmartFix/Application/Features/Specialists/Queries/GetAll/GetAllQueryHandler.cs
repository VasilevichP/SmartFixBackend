using MediatR;
using SmartFix.Application.Features.Specialists.DTO;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Specialists.Queries.GetAll;

public class GetAllSpecialistsQueryHandler : IRequestHandler<GetAllQuery, List<SpecialistDto>>
{
    private readonly ISpecialistRepository _specialistRepository;

    public GetAllSpecialistsQueryHandler(ISpecialistRepository specialistRepository)
    {
        _specialistRepository = specialistRepository;
    }

    public async Task<List<SpecialistDto>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        var specialists = await _specialistRepository.GetAllAsync(cancellationToken);
        return specialists
            .Select(s => new SpecialistDto { Id = s.Id, FullName = s.FullName })
            .ToList();
    }
}