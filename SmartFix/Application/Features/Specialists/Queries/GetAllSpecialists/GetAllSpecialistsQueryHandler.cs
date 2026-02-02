using MediatR;
using SmartFix.Application.Features.Specialists.DTO;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Specialists.Queries.GetAllSpecialists;

public class GetAllSpecialistsQueryHandler : IRequestHandler<GetAllSpecialistsQuery, List<SpecialistDto>>
{
    private readonly ISpecialistRepository _specialistRepository;

    public GetAllSpecialistsQueryHandler(ISpecialistRepository specialistRepository)
    {
        _specialistRepository = specialistRepository;
    }

    public async Task<List<SpecialistDto>> Handle(GetAllSpecialistsQuery request, CancellationToken cancellationToken)
    {
        var specialists = await _specialistRepository.GetAllAsync(cancellationToken);
        return specialists
            .Select(s => new SpecialistDto { Id = s.Id, FullName = s.FullName })
            .ToList();
    }
}