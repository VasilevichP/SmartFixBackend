using System.Net;
using SmartFix.Domain.Exceptions;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Domain.Aggregates;

public class PromoCode
{
    public Guid Id { get; private set; }
    public string Code { get; private set; }
    public DiscountType Type { get; private set; }
    public decimal Value { get; private set; }
    
    public DateTime ExpirationDate { get; private set; }
    public int UsageLimit { get; private set; }
    public bool IsActive { get; private set; }

    private PromoCode() { }

    public static PromoCode Create(string code, DiscountType type, decimal value, DateTime expirationDate, int usageLimit)
    {
        if (string.IsNullOrWhiteSpace(code)) throw new HttpException(HttpStatusCode.BadRequest,"Промокод не может быть пустым.");
        if (value <= 0) throw new HttpException(HttpStatusCode.BadRequest,"Значение скидки должно быть больше 0.");
        if (type == DiscountType.Percent && (value < 0 || value > 100))
            throw new HttpException(HttpStatusCode.BadRequest, "Процентное значение скидки должно быть от 1 до 100");
        
        return new PromoCode
        {
            Id = Guid.NewGuid(),
            Code = code.ToUpper(),
            Type = type,
            Value = value,
            ExpirationDate = expirationDate,
            UsageLimit = usageLimit,
            IsActive = true
        };
    }
    
    public void Update(string code, DiscountType type, decimal value, DateTime expirationDate, int usageLimit)
    {
        if (string.IsNullOrWhiteSpace(code)) throw new Exception("Промокод не может быть пустым.");
        if (value <= 0) throw new Exception("Значение скидки должно быть больше 0.");
        if (type == DiscountType.Percent && (value < 0 || value > 100))
            throw new HttpException(HttpStatusCode.BadRequest, "Процентное значение скидки должно быть от 1 до 100");
        
        Code = code.ToUpper();
        Type = type;
        Value = value;
        ExpirationDate = expirationDate;
        UsageLimit = usageLimit;
    }
    
    public void ChangeStatus() => IsActive = !IsActive;

    public bool IsValid() => UsageLimit > 0 && ExpirationDate > DateTime.UtcNow && IsActive;
    public void DecrementLimit()
    {
        if (UsageLimit > 0) UsageLimit--;
    }
}