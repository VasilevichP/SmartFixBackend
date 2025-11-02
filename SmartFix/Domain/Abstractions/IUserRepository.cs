using SmartFix.Domain.Aggregates;

namespace SmartFix.Domain.Abstractions;

public interface IUserRepository
{
    public Task AddAsync(User user, CancellationToken cancellationToken = default);
    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<bool> FindByEmailAsync(string email, CancellationToken cancellationToken = default);
    public Task<bool> DoesManagerExistAsync(CancellationToken cancellationToken = default);
}