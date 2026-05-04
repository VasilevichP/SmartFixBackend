using MediatR;
using SmartFix.Application.Features.Masters.DTO;

namespace SmartFix.Application.Features.Masters.Queries.GetMasterById;

public class GetMasterByIdQuery:IRequest<MasterDetailsDto>
{
    public Guid Id { get; set; }
}