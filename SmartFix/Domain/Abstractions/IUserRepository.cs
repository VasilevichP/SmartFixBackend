using SmartFix.Domain.Aggregates;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Domain.Abstractions;

public interface IUserRepository
{
    public Task AddAsync(User user, CancellationToken cancellationToken = default);
    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<Client?> GetClientByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<Master?> GetMasterByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<bool> FindByEmailAsync(string email, CancellationToken cancellationToken = default);
    public void Update(User user, CancellationToken cancellationToken = default);
    Task<List<Client>> GetClientsListAsync(
        string? nameSearch, 
        string? phoneSearch, 
        ClientStatus? status, 
        int sortOrder, 
        CancellationToken cancellationToken = default);
    
    Task<List<Master>> GetMasterListAsync(
        string? nameSearch,
        string? phoneSearch,
        CancellationToken cancellationToken = default);
    
    Task<int> CountMasterActiveRequestsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<int> GetClientOrdersCountAsync(Guid clientId, CancellationToken ct);
}