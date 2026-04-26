using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Features.PromoCodes.DTO;

public class PromoCodeDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public DateTime ExpirationDate { get; set; }
    public bool IsActive { get; set; }
    public bool IsValid { get; set; }
}