using System.ComponentModel.DataAnnotations;
using MediatR;

namespace SmartFix.Application.Features.Users.Commands.UpdateProfileByClient;

public class UpdateProfileByClientCommand : IRequest
{
    public Guid Id { get; set; }

    [EmailAddress(ErrorMessage = "Некорректный формат Email")]
    public string Email { get; set; }

    public string Name { get; set; }

    [Phone(ErrorMessage = "Некорректный формат номера телефона")]
    public string Phone { get; set; }
}