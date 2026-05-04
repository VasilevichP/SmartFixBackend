using MediatR;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Application.Features.PromoCodes.Commands.CreatePromoCode;

public class CreatePromoCodeCommand: IRequest
{
    public string Code { get; set; } = string.Empty;
    public DiscountType Type { get; set; }
    public decimal Value { get; set; }
    public DateTime ExpirationDate { get; set; }
    public int UsageLimit { get; set; }
}