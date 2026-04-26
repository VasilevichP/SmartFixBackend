using MediatR;

namespace SmartFix.Application.Features.Discounts.Commands.ChangeDiscountStatus;

public class ChangeDiscountStatusCommand:IRequest
{
    public Guid Id { get; set; }
}