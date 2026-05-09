using MediatR;
using SmartFix.Application.Features.Chats.DTO;

namespace SmartFix.Application.Features.Chats.Queries.GetChatHistory;

public class GetChatHistoryQuery:IRequest<List<ChatMessageDto>>
{
    public Guid ClientId { get; set; }
}