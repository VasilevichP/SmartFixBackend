using MediatR;

namespace SmartFix.Application.Features.Specialists.Commands.DeleteSpecialist;

public class DeleteSpecialistCommand : IRequest
{
    public Guid Id { get; set; }
}