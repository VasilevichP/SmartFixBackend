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

    private RequestService() { }

    public static RequestService Create(Guid requestId, Service service)
    {
        return new RequestService
        {
            RequestId = requestId,
            ServiceId = service.Id,
            ServiceName = service.Name,
            Price = service.Price
        };
    }
    public static RequestService Create(Guid requestId, string name,decimal price)
    {
        if (string.IsNullOrWhiteSpace(name)) 
            throw new HttpException(HttpStatusCode.BadRequest,"Название работы не может быть пустым.");
        
        if (price < 0) 
            throw new HttpException(HttpStatusCode.BadRequest,"Цена работы не может быть отрицательной.");
        return new RequestService
        {
            RequestId = requestId,
            ServiceName = name,
            Price = price
        };
    }
}