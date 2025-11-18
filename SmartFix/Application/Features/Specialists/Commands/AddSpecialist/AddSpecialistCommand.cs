using MediatR;

namespace SmartFix.Application.Features.Specialists.Commands.AddSpecialist;

public class AddSpecialistCommand : IRequest
{
    public string FullName { get; set; }
}