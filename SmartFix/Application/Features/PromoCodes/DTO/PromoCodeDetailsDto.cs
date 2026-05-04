using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Features.PromoCodes.DTO;

public class PromoCodeDetailsDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public DiscountType Type { get; set; }
    public decimal Value { get; set; }
    public DateTime ExpirationDate { get; set; }
    public int UsageLimit { get; set; }
    public bool IsActive { get; set; }
    public bool IsValid { get; set; }
}