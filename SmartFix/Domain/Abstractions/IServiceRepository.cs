using SmartFix.Application.Features.Requests.DTO;
using SmartFix.Domain.Aggregates;

namespace SmartFix.Domain.Abstractions;

public interface IServiceRepository
{
    Task AddAsync(Service service, CancellationToken cancellationToken = default);
    Task<Service?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Update(Service service);
    Task<List<ServiceForRequestDto>> GetAllForRequestAsync(CancellationToken cancellationToken = default);
    Task<List<Service>> GetFilteredForManagerAsync(
        string? searchTerm,
        bool? status,
        Guid? categoryId,
        Guid? deviceTypeId,
        Guid? manufacturerId,
        Guid? deviceModelId, 
        int sortOrder,
        CancellationToken cancellationToken = default);
    
    Task<List<Service>> GetFilteredForClientAsync(
        string? searchTerm,
        Guid? categoryId,
        Guid? deviceTypeId,
        Guid? manufacturerId,
        Guid? deviceModelId, 
        int sortOrder,
        CancellationToken cancellationToken = default);

    public Task<bool> IsDuplicateAsync(Guid? id, string name, Guid deviceTypeId, Guid? deviceModelId, CancellationToken cancellationToken = default);
    public Task<bool> HasLinkedRequestsAsync(Guid serviceId, CancellationToken cancellationToken = default);
    public Task Delete(Service service, CancellationToken cancellationToken = default);
}