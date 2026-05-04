using MediatR;
using SmartFix.Application.Features.Masters.DTO;

namespace SmartFix.Application.Features.Masters.Queries.GetAllMasters;

public class GetAllMastersQuery:IRequest<List<MasterDto>>
{
    public string? nameSearch { get; set; } 
    public string? phoneSearch { get; set; } 
}