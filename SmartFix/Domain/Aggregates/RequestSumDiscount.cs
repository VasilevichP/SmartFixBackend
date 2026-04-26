using System.Net;
using SmartFix.Domain.Exceptions;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Domain.Aggregates;

public class RequestSumDiscount: Discount
{
    public decimal TargetSum { get; private set; }

    private RequestSumDiscount() { }

    public static RequestSumDiscount Create(string name, decimal targetSum, DiscountType type, decimal value, int priority)
    {
        if (type == DiscountType.Percent && (value < 0 || value > 100))
            throw new HttpException(HttpStatusCode.BadRequest, "Процентное значение скидки должно быть от 1 до 100");
        return new RequestSumDiscount
        {
            Id = Guid.NewGuid(), Name = name, Priority = priority, IsActive = true,
            Type = type, Value = value, TargetSum = targetSum
        };
    }

    public override bool IsApplicable(Request request, int clientTotalOrders)
    {
        return IsActive && request.BasePrice >= TargetSum;
    }
    
    public void Update(string name, decimal targetSum, DiscountType type, decimal value, int priority)
    {
        UpdateBase(name, type, value, priority);
        TargetSum = targetSum;
    }
}