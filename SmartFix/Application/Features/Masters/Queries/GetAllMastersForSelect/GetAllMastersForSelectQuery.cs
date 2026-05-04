using MediatR;
using SmartFix.Application.Features.Masters.DTO;

namespace SmartFix.Application.Features.Masters.Queries.GetAllMastersForSelect;

public class GetAllMastersForSelectQuery:IRequest<List<MasterSelectDto>>;