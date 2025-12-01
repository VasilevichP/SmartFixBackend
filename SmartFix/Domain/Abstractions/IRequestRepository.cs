using SmartFix.Domain.Aggregates;

namespace SmartFix.Domain.Abstractions;

public interface IRequestRepository
{
    void Update (Request request);
    Task AddAsync(Request request, CancellationToken cancellationToken = default);
    Task<Request?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Request>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<Request>> GetAllForClientAsync(Guid clientId, CancellationToken cancellationToken = default);
}