using MediatR;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Features.Discounts.Commands.UpdateDiscount;

public class UpdateDiscountCommand: IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DiscountType Type { get; set; }
    public decimal Value { get; set; }
    public int Priority { get; set; }
    
    public string ConditionValue { get; set; } = string.Empty; 
}