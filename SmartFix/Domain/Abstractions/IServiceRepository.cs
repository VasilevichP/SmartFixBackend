using SmartFix.Domain.Aggregates;

namespace SmartFix.Domain.Abstractions;

public interface IServiceRepository
{
    Task AddAsync(Service service, CancellationToken cancellationToken = default);
    Task<Service?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Update(Service service); 
    Task<List<Service>> GetAllForClientAsync(CancellationToken cancellationToken = default);
    Task<List<Service>> GetAllForManagerAsync(CancellationToken cancellationToken = default);
}   