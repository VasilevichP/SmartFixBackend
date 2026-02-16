using SmartFix.Domain.Aggregates;

namespace SmartFix.Domain.Abstractions;

public interface IDeviceTypeRepository
{
    Task<DeviceType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<DeviceType>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(DeviceType deviceType, CancellationToken cancellationToken = default);
    void Update (DeviceType deviceType);
    Task<bool> ExistsByName(String name, CancellationToken ct);
    Task<bool> HasRelatedModelsAsync(Guid id, CancellationToken ct);
    Task<bool> HasRelatedServicesAsync(Guid id, CancellationToken ct);
    Task<bool> HasRelatedRequestsAsync(Guid id, CancellationToken ct); 
    void Delete(DeviceType deviceType);
}