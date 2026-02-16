using SmartFix.Domain.Aggregates;

namespace SmartFix.Domain.Abstractions;

public interface IServiceCategoryRepository
{
    Task<ServiceCategory?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<ServiceCategory>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(ServiceCategory category, CancellationToken cancellationToken = default);
    Task<bool> ExistsByName(String name, CancellationToken ct);
    Task<bool> HasRelatedServicesAsync(Guid id, CancellationToken ct);
    void Update(ServiceCategory category);
    void Delete(ServiceCategory category);
}