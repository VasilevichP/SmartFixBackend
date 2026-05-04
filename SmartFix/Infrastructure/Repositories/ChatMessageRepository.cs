using Microsoft.EntityFrameworkCore;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Infrastructure.Persistence;

namespace SmartFix.Infrastructure.Repositories;

public class ChatMessageRepository : IChatMessageRepository
{
    private readonly AppDbContext _context;

    public ChatMessageRepository(AppDbContext context) => _context = context;

    public async Task AddAsync(ChatMessage message, CancellationToken cancellationToken = default)
    {
        await _context.ChatMessages.AddAsync(message, cancellationToken);
    }
    public async Task<List<ChatMessage>> GetChatHistoryAsync(Guid clientId, CancellationToken cancellationToken = default)
    {
        return await _context.ChatMessages
            .Include(c => c.Client)
            .Where(c => c.ClientId == clientId)
            .OrderBy(c => c.SentAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetUnreadCountForClientAsync(Guid clientId, CancellationToken cancellationToken = default)
    {
        return await _context.ChatMessages
            .CountAsync(c => c.ClientId == clientId && !c.IsFromClient && !c.IsRead, cancellationToken);
    }

    public async Task<int> GetUnreadCountForManagersAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ChatMessages
            .CountAsync(c => c.IsFromClient && !c.IsRead, cancellationToken);
    }

    public async Task MarkMessagesAsReadAsync(Guid clientId, bool readByClient, CancellationToken cancellationToken = default)
    {
        var unreadMessages = await _context.ChatMessages
            .Where(c => c.ClientId == clientId 
                     && c.IsFromClient != readByClient 
                     && !c.IsRead)
            .ToListAsync(cancellationToken);

        foreach (var message in unreadMessages)
        {
            message.MarkAsRead();
        }
    }
}