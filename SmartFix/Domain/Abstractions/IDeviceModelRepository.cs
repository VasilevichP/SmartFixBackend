using SmartFix.Domain.Aggregates;

namespace SmartFix.Domain.Abstractions;

public interface IDeviceModelRepository
{
    Task<DeviceModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<DeviceModel>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<DeviceModel>> GetAllByTypeAndManufacturerAsync(Guid? typeId, Guid? manufacturerId, CancellationToken cancellationToken = default);
    Task<bool> IsUsedInSystemAsync(Guid id, CancellationToken ct);
    Task AddAsync(DeviceModel deviceModel, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAndManufacturer(String name,Guid manufacturerId, CancellationToken ct);
    void Update(DeviceModel deviceModel);
    Task Delete(DeviceModel deviceModel, CancellationToken cancellationToken = default);
}