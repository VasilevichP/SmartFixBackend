using MediatR;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Features.Discounts.Commands.CreateDiscount;

public class CreateDiscountCommand: IRequest
{
    public string Name { get; set; } = string.Empty;
    public DiscountCategory Category { get; set; }
    public string ConditionValue { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public DiscountType Type { get; set; }
    public int Priority { get; set; }
}