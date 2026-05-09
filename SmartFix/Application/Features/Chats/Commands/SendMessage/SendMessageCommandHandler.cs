using MediatR;
using Microsoft.AspNetCore.SignalR;
using SmartFix.Api.Hubs;
using SmartFix.Application.Features.Chats.DTO;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;

namespace SmartFix.Application.Features.Chats.Commands.SendMessage;

public class SendMessageCommandHandler: IRequestHandler<SendMessageCommand, ChatMessageDto>
{
    private readonly IChatMessageRepository _chatRepo;
    private readonly IUserRepository _userRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHubContext<ChatHub> _hubContext;

    public SendMessageCommandHandler(IChatMessageRepository chatRepo, IUserRepository userRepo, IUnitOfWork uow, IHubContext<ChatHub> hubContext)
    {
        _chatRepo = chatRepo; _userRepo = userRepo; _unitOfWork = uow; _hubContext = hubContext;
    }

    public async Task<ChatMessageDto> Handle(SendMessageCommand request, CancellationToken ct)
    {
        var client = await _userRepo.GetClientByIdAsync(request.ClientId, ct);
        if (client == null) throw new Exception("Клиент не найден");

        ChatMessage message = ChatMessage.Create(request.ClientId, request.IsFromClient, request.Text);;
        string senderName = client.Name ?? "Клиент";

        if (!request.IsFromClient)
        {
            senderName = "Служба поддержки";
        }

        await _chatRepo.AddAsync(message, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        var dto = new ChatMessageDto
        {
            Id = message.Id, ClientId = message.ClientId, MessageText = message.MessageText,
            IsFromClient = message.IsFromClient, SentAt = message.SentAt, SenderName = senderName
        };

        if (request.IsFromClient)
        {
            await _hubContext.Clients.Group("Managers").SendAsync("ReceiveMessage", dto, cancellationToken: ct);
        }
        else
        {
            await _hubContext.Clients.User(request.ClientId.ToString()).SendAsync("ReceiveMessage", dto, cancellationToken: ct);
        }

        return dto;
    }
}