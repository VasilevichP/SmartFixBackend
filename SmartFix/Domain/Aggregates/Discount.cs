using System.Net;
using SmartFix.Domain.Exceptions;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Domain.Aggregates;

public abstract class Discount
{
    public Guid Id { get; protected set; }
    public string Name { get; protected set; }
    public int Priority { get; protected set; }
    public bool IsActive { get; protected set; }

    public DiscountType Type { get; protected set; }
    public decimal Value { get; protected set; }

    protected Discount()
    {
    }

    public abstract bool IsApplicable(Request request, int clientTotalOrders);

    public void ChangeStatus() => IsActive = !IsActive;

    protected void UpdateBase(string name, DiscountType type, decimal value, int priority)
    {
        if (type == DiscountType.Percent && (value < 0 || value > 100))
            throw new HttpException(HttpStatusCode.BadRequest, "Процентное значение скидки должно быть от 1 до 100");
        Name = name;
        Type = type;
        Value = value;
        Priority = priority;
    }
}