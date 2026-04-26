using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Features.Discounts.DTO;

public class DiscountDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DiscountCategory Category { get; set; }
    public DiscountType Type { get; set; }
    public bool IsActive { get; set; }
}