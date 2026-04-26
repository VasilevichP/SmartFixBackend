using System.Net;
using SmartFix.Domain.Exceptions;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Domain.Aggregates;

public class DayOfWeekDiscount: Discount
{
    public DayOfWeek TargetDayOfWeek { get; private set; }

    private DayOfWeekDiscount() { }

    public static DayOfWeekDiscount Create(string name, DayOfWeek dayOfWeek, DiscountType type, decimal value, int priority)
    {
        if (type == DiscountType.Percent && (value < 0 || value > 100))
            throw new HttpException(HttpStatusCode.BadRequest, "Процентное значение скидки должно быть от 1 до 100");
        return new DayOfWeekDiscount
        {
            Id = Guid.NewGuid(), Name = name, Priority = priority, IsActive = true,
            Type = type, Value = value, TargetDayOfWeek = dayOfWeek
        };
    }

    public override bool IsApplicable(Request request, int clientTotalOrders)
    {
        return IsActive && request.CreatedAt.DayOfWeek == TargetDayOfWeek;
    }
    
    public void Update(string name, DayOfWeek dayOfWeek, DiscountType type, decimal value, int priority)
    {
        UpdateBase(name, type, value, priority);
        TargetDayOfWeek = dayOfWeek;
    }
}