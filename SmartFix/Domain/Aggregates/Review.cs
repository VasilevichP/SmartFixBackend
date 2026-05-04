using System.Net;
using SmartFix.Domain.Exceptions;

namespace SmartFix.Domain.Aggregates;

public class Review
{
    public Guid Id { get; private set; }
    public int Rating { get; private set; }
    public string? Comment { get; private set; }
    
    public Guid RequestId { get; private set; }
    public Request Request { get; private set; } 
    
    public Guid ClientId { get; private set; }
    public User Client { get; private set; }

    private Review() { }

    public static Review Create(Guid requestId, Guid clientId, int rating, string? comment)
    {
        if (rating < 1 || rating > 5)
            throw new HttpException(HttpStatusCode.BadRequest,"Рейтинг должен быть от 1 до 5.");

        return new Review
        {
            RequestId = requestId,
            ClientId = clientId,
            Rating = rating,
            Comment = comment,
        };
    }
}