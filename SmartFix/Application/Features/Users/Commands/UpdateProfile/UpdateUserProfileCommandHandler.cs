using System.Net;
using MediatR;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Application.Features.Users.Commands.UpdateProfile;

public class UpdateUserProfileCommandHandler: IRequestHandler<UpdateUserProfileCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;


    public UpdateUserProfileCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user == null)
        {
            throw new HttpException(HttpStatusCode.NotFound, "Пользователь не найден");
        }
        if (!user.Email.Equals(request.Email) && _userRepository.FindByEmailAsync(request.Email,cancellationToken).Result)
        {
            throw new HttpException(HttpStatusCode.BadRequest, "Пользователь с таким email уже существует.");
        }

        user.UpdateUser(request.Email, request.Name, request.Phone);
        _userRepository.Update(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}