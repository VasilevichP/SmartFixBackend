using MediatR;

namespace SmartFix.Application.Features.PromoCodes.Commands.ChangePromoCodeStatus;

public class ChangePromoCodeStatusCommand: IRequest
{
    public Guid Id { get; set; }
}