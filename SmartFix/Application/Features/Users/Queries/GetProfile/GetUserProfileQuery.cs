using MediatR;
using SmartFix.Application.Features.Users.DTO;

namespace SmartFix.Application.Features.Users.Queries.GetProfile;

public class GetUserProfileQuery : IRequest<ProfileDTO>
{
    public Guid UserId { get; set; }
}