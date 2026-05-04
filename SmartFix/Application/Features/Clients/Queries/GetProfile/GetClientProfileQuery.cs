using MediatR;
using SmartFix.Application.Features.Clients.DTO;

namespace SmartFix.Application.Features.Clients.Queries.GetProfile;

public class GetClientProfileQuery : IRequest<ClientProfileDto>
{
    public Guid UserId { get; set; }
}