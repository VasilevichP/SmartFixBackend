using MediatR;
using SmartFix.Application.Features.Specialists.DTO;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Specialists.Queries.GetAllWithLoad;

public class GetAllSpecialistsWithLoadQueryHandler: IRequestHandler<GetAllSpecialistsWithLoadQuery, List<SpecialistWithLoadDto>>
{
    private readonly ISpecialistRepository _specialistRepository;

    public GetAllSpecialistsWithLoadQueryHandler(ISpecialistRepository specialistRepository)
    {
        _specialistRepository = specialistRepository;
    }

    public async Task<List<SpecialistWithLoadDto>> Handle(GetAllSpecialistsWithLoadQuery request, CancellationToken cancellationToken)
    {
        var specialists = await _specialistRepository.GetAllWithLoadAsync(cancellationToken);
        return specialists
            .Select(s => new SpecialistWithLoadDto() { Id = s.Specialist.Id, Name = s.Specialist.Name, ActiveRequestsCount = s.Load})
            .ToList();
    }
}