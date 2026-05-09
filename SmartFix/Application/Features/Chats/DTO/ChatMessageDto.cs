namespace SmartFix.Application.Features.Chats.DTO;

public class ChatMessageDto
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public string MessageText { get; set; } = string.Empty;
    public bool IsFromClient { get; set; }
    public DateTime SentAt { get; set; }
    public string SenderName { get; set; } = string.Empty;
}