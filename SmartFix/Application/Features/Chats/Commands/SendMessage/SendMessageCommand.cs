using MediatR;
using SmartFix.Application.Features.Chats.DTO;

namespace SmartFix.Application.Features.Chats.Commands.SendMessage;

public class SendMessageCommand: IRequest<ChatMessageDto>
{
    public Guid ClientId { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsFromClient { get; set; }
}
