using MediatR;
using SmartFix.Application.Features.Clients.DTO;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Features.Clients.Queries.GetAllClients;

public class GetAllClientsQuery: IRequest<List<ClientBriefDto>> 
{ 
    public string? nameSearch { get; set; } 
    public string? phoneSearch { get; set; } 
    public ClientStatus? Status { get; set; }
    public int SortOrder { get; set; }
}