using MediatR;
using SmartFix.Application.Features.Specialists.DTO;

namespace SmartFix.Application.Features.Specialists.Queries.GetAllSpecialists;

public class GetAllSpecialistsQuery : IRequest<List<SpecialistDto>>
{
}