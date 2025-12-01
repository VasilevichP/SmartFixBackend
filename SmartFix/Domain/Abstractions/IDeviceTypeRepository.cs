using SmartFix.Domain.Aggregates;

namespace SmartFix.Domain.Abstractions;

public interface IDeviceTypeRepository
{
    Task<DeviceType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<DeviceType>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(DeviceType deviceType, CancellationToken cancellationToken = default);
    void Update (DeviceType deviceType);
    void Delete(DeviceType deviceType);
}