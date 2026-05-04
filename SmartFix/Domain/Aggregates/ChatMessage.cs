namespace SmartFix.Domain.Aggregates;

public class ChatMessage
{
    public Guid Id { get; private set; }
    
    public Guid ClientId { get; private set; }
    public Client Client { get; private set; }
   
    public bool IsFromClient { get; private set; }
    public string MessageText { get; private set; }
    public DateTime SentAt { get; private set; }
    public bool IsRead { get; private set; }

    private ChatMessage() { }

    public static ChatMessage Create(Guid clientId, bool isFromClient, string text)
    {
        return new ChatMessage
        {
            Id = Guid.NewGuid(),
            ClientId = clientId,
            IsFromClient = isFromClient,
            MessageText = text,
            SentAt = DateTime.UtcNow,
            IsRead = false
        };
    }

    public void MarkAsRead() => IsRead = true;
}