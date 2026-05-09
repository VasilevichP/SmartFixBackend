using MediatR;
using SmartFix.Application.Features.Chats.DTO;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Chats.Queries.GetChatHistory;

public class GetChatHistoryQueryHandler:IRequestHandler<GetChatHistoryQuery,List<ChatMessageDto>>
{
    private readonly IChatMessageRepository _chatMessageRepository;

    public GetChatHistoryQueryHandler(IChatMessageRepository chatMessageRepository)
    {
        _chatMessageRepository = chatMessageRepository;
    }

    public async Task<List<ChatMessageDto>> Handle(GetChatHistoryQuery request, CancellationToken cancellationToken)
    {
        var messages = await _chatMessageRepository.GetChatHistoryAsync(request.ClientId, cancellationToken);
        return messages.Select(m=> new ChatMessageDto
        {
            Id = m.Id,
            ClientId = m.ClientId,
            MessageText = m.MessageText,
            IsFromClient = m.IsFromClient,
            SenderName = m.IsFromClient? "Клиент":"Менеджер",
            SentAt = m.SentAt
        }).ToList();
    }
}