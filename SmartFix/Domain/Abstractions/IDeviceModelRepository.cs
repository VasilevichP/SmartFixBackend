using SmartFix.Domain.Aggregates;

namespace SmartFix.Domain.Abstractions;

public interface IDeviceModelRepository
{
    Task<DeviceModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<DeviceModel>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<DeviceModel>> GetAllByTypeAndManufacturerAsync(Guid? typeId, Guid? manufacturerId, CancellationToken cancellationToken = default);
    Task AddAsync(DeviceModel deviceModel, CancellationToken cancellationToken = default);
    void Update(DeviceModel deviceModel);
    void Delete(DeviceModel deviceModel);
}