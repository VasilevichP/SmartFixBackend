using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Features.Discounts.DTO;

public class DiscountDetailsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DiscountCategory Category { get; set; }
    public string ConditionValue { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public DiscountType Type { get; set; }
    public int Priority { get; set; }
    public bool IsActive { get; set; }
}