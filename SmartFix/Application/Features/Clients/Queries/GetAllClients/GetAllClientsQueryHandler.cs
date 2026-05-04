using MediatR;
using SmartFix.Application.Features.Clients.DTO;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Features.Clients.Queries.GetAllClients;

public class GetAllClientsQueryHandler: IRequestHandler<GetAllClientsQuery, List<ClientBriefDto>>
{
    private readonly IUserRepository _userRepository;

    public GetAllClientsQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<ClientBriefDto>> Handle(GetAllClientsQuery request, CancellationToken ct)
    {
        var clients = await _userRepository.GetClientsListAsync(
            request.nameSearch, 
            request.phoneSearch,
            request.Status, 
            request.SortOrder, 
            ct);

        return clients.Select(c => new ClientBriefDto
        {
            Id = c.Id,
            Name = c.Name,
            Email = c.Email,
            Phone = c.PhoneNumber,
            Status = c.Status
        }).ToList();
    }
}