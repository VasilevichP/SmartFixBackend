using MediatR;

namespace SmartFix.Application.Features.Services.Commands.ChangeVisibility;

public class ToggleServiceVisibilityCommand : IRequest
{
    public Guid Id { get; set; } 
}