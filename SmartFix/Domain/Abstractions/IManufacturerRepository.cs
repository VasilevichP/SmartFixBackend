using SmartFix.Domain.Aggregates;

namespace SmartFix.Domain.Abstractions;

public interface IManufacturerRepository
{
    Task<Manufacturer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Manufacturer>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Manufacturer manufacturer, CancellationToken cancellationToken = default);
    void Update(Manufacturer manufacturer);
    void Delete(Manufacturer manufacturer);
}