using MediatR;
using SmartFix.Application.Features.Specialists.DTO;

namespace SmartFix.Application.Features.Specialists.Queries.GetAll;

public class GetAllSpecialistsQuery : IRequest<List<SpecialistDto>>
{
}