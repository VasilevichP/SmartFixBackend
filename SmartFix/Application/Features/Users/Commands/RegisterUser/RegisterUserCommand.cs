using System.ComponentModel.DataAnnotations;
using MediatR;

namespace SmartFix.Application.Features.Users.Commands.RegisterUser;

public class RegisterUserCommand : IRequest
{
    public string Name { get; set; }
    [EmailAddress(ErrorMessage = "Некорректный формат Email")]
    public string Email { get; set; }
    [Phone(ErrorMessage = "Некорректный формат номера телефона")]
    public string Phone { get; set; }
    public string Password { get; set; }
}