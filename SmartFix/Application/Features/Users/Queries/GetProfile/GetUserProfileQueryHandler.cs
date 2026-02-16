using System.Net;
using MediatR;
using SmartFix.Application.Authentication;
using SmartFix.Application.Features.Users.Commands.AuthorizeUser;
using SmartFix.Application.Features.Users.DTO;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.Users.Queries.GetProfile;

public class GetUserProfileQueryHandler: IRequestHandler<GetUserProfileQuery, ProfileDTO>
{
    private readonly IUserRepository _userRepository;

    public GetUserProfileQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ProfileDTO> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            throw new HttpException(HttpStatusCode.NotFound, "Пользователь не найден");
        }

        var profileDTO = new ProfileDTO()
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            Phone = user.PhoneNumber
        };
        return profileDTO;
    }
}