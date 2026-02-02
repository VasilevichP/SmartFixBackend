namespace SmartFix.Domain.Aggregates;

public class Review
{
    public Guid Id { get; private set; }
    public int Rating { get; private set; }
    public string? Comment { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    public Guid ServiceId { get; private set; }
    public Service Service { get; private set; } 
    
    public Guid ClientId { get; private set; }
    public User Client { get; private set; }

    private Review() { }

    public static Review Create(Guid serviceId, Guid clientId, int rating, string comment)
    {
        if (rating < 1 || rating > 5)
            throw new ArgumentException("Рейтинг должен быть от 1 до 5.");

        return new Review
        {
            Id = Guid.NewGuid(),
            ServiceId = serviceId,
            ClientId = clientId,
            Rating = rating,
            Comment = comment,
            CreatedAt = DateTime.UtcNow
        };
    }
}