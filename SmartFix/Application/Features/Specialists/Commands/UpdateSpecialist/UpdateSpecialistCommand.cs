using MediatR;
using SmartFix.Domain.Aggregates;

namespace SmartFix.Application.Features.Specialists.Commands.UpdateSpecialist;

public class UpdateSpecialistCommand: IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}