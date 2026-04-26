using System.Net;
using MediatR;
using SmartFix.Application.Authentication;
using SmartFix.Application.Features.Clients.DTO;
using SmartFix.Application.Features.Users.Commands.AuthorizeUser;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.Clients.Queries.GetProfile;

public class GetClientProfileQueryHandler: IRequestHandler<GetClientProfileQuery, ClientProfileDto>
{
    private readonly IUserRepository _userRepository;

    public GetClientProfileQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ClientProfileDto> Handle(GetClientProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetClientByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            throw new HttpException(HttpStatusCode.NotFound, "Пользователь не найден");
        }

        return new ClientProfileDto()
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            Phone = user.PhoneNumber,
            Status = user.Status,
            PersonalDiscount = user.PersonalDiscount,
        };
    }
}