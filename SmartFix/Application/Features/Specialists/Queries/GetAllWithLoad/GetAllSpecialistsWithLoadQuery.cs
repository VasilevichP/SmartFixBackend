using MediatR;
using SmartFix.Application.Features.Specialists.DTO;
using SmartFix.Domain.Aggregates;

namespace SmartFix.Application.Features.Specialists.Queries.GetAllWithLoad;

public class GetAllSpecialistsWithLoadQuery: IRequest<List<SpecialistWithLoadDto>>
{
    
}