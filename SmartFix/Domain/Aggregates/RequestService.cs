using System.Net;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Domain.Aggregates;

public class RequestService
{
    public Guid Id { get; private set; }
    public Guid RequestId { get; private set; }
    public Request Request { get; private set; }

    public Guid? ServiceId { get; private set; }
    public Service? Service { get; private set; }
    public string ServiceName { get; private set; }
    public decimal Price { get; private set; }

    public int? WarrantyPeriodMonths { get; private set; }
    public DateTime? WarrantyEndDate { get; private set; }

    private RequestService()
    {
    }

    public static RequestService Create(Guid requestId, Guid? serviceId, string name, decimal price, int? warrantyPeriodMonths)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new HttpException(HttpStatusCode.BadRequest, "Название работы не может быть пустым.");

        if (price < 0)
            throw new HttpException(HttpStatusCode.BadRequest, "Цена работы не может быть отрицательной.");
        return new RequestService
        {
            Id = Guid.NewGuid(),
            RequestId = requestId,
            ServiceId = serviceId,
            ServiceName = name,
            Price = price,
            WarrantyPeriodMonths = warrantyPeriodMonths
        };
    }
    
    public void StartWarranty(DateTime closedAt)
    {
        if (WarrantyPeriodMonths.HasValue && WarrantyPeriodMonths.Value > 0)
        {
            WarrantyEndDate = closedAt.AddMonths(WarrantyPeriodMonths.Value);
        }
    }
}