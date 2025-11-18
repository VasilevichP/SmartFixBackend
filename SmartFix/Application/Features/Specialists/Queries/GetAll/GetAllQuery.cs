using MediatR;
using SmartFix.Application.Features.Specialists.DTO;

namespace SmartFix.Application.Features.Specialists.Queries.GetAll;

public class GetAllQuery : IRequest<List<SpecialistDto>>
{
}