using SmartFix.Domain.Aggregates;

namespace SmartFix.Domain.Abstractions;

public interface IChatMessageRepository
{
    Task AddAsync(ChatMessage message, CancellationToken cancellationToken = default);
    Task<List<ChatMessage>> GetChatHistoryAsync(Guid clientId, CancellationToken cancellationToken = default);
    Task<int> GetUnreadCountForClientAsync(Guid clientId, CancellationToken cancellationToken = default);
    Task<int> GetUnreadCountForManagersAsync(CancellationToken cancellationToken = default);
    Task MarkMessagesAsReadAsync(Guid clientId, bool readByClient, CancellationToken cancellationToken = default);
}