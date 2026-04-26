using System.Net;
using SmartFix.Domain.Exceptions;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Domain.Aggregates;

public class RequestsCountDiscount: Discount
{
    public int TargetOrdersCount { get; private set; }

    private RequestsCountDiscount() { }

    public static RequestsCountDiscount Create(string name, int targetOrders, DiscountType type, decimal value, int priority)
    {
        if (type == DiscountType.Percent && (value < 0 || value > 100))
            throw new HttpException(HttpStatusCode.BadRequest, "Процентное значение скидки должно быть от 1 до 100");
        return new RequestsCountDiscount
        {
            Id = Guid.NewGuid(), Name = name, Priority = priority, IsActive = true,
            Type = type, Value = value, TargetOrdersCount = targetOrders
        };
    }

    public override bool IsApplicable(Request request, int clientTotalOrders)
    {
        return IsActive && clientTotalOrders >= TargetOrdersCount;
    }
    
    public void Update(string name, int targetOrders, DiscountType type, decimal value, int priority)
    {
        UpdateBase(name, type, value, priority);
        TargetOrdersCount = targetOrders;
    }
}