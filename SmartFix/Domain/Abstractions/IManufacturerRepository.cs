using SmartFix.Domain.Aggregates;

namespace SmartFix.Domain.Abstractions;

public interface IManufacturerRepository
{
    Task<Manufacturer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Manufacturer>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Manufacturer manufacturer, CancellationToken cancellationToken = default);
    Task<bool> ExistsByName(String name, CancellationToken ct);
    Task<bool> HasRelatedModelsAsync(Guid id, CancellationToken ct);
    Task<bool> HasRelatedServicesAsync(Guid id, CancellationToken ct);
    void Update(Manufacturer manufacturer);
    void Delete(Manufacturer manufacturer);
}