using Microsoft.EntityFrameworkCore;
using SmartFix.Application.Features.Requests.DTO;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Infrastructure.Persistence;

namespace SmartFix.Infrastructure.Repositories;

public class ServiceRepository : IServiceRepository
{
    private readonly AppDbContext _context;

    public ServiceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Service service, CancellationToken cancellationToken = default)
    {
        await _context.Services.AddAsync(service, cancellationToken);
    }

    public async Task<Service?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Services
            .Include(s => s.Category)
            .Include(s => s.DeviceModel)
            .ThenInclude(s => s.Manufacturer)
            .Include(s => s.DeviceType)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public void Update(Service service)
    {
        _context.Services.Update(service);
    }

    public async Task<List<ServiceForRequestDto>> GetAllForRequestAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Services
            .Where(s => s.IsAvailable && !s.IsDeleted)
            .Select(s => new ServiceForRequestDto
            {
                ServiceId = s.Id,
                ServiceName = s.Name,
                Price = s.Price
            })
            .ToListAsync(cancellationToken);
    }

    private IQueryable<Service> ApplyFilters(IQueryable<Service> query, string? searchTerm, Guid? categoryId,
        Guid? deviceTypeId, Guid? manufacturerId, Guid? deviceModelId, bool? status, int sortOrder)
    {
        if (!string.IsNullOrWhiteSpace(searchTerm))
            query = query.Where(s => s.Name.ToLower().Contains(searchTerm) || s.Category.Name.Contains(searchTerm));

        if (categoryId.HasValue)
            query = query.Where(s => s.CategoryId == categoryId.Value);
        
        if (status.HasValue)
            query = query.Where(s => s.IsAvailable == status.Value);

        if (deviceTypeId.HasValue)
            query = query.Where(s => s.DeviceTypeId == deviceTypeId.Value);

        if (manufacturerId.HasValue)
            query = query.Where(s =>
                s.ManufacturerId == manufacturerId.Value ||
                (s.DeviceModel != null && s.DeviceModel.ManufacturerId == manufacturerId.Value)
            );
        ;

        if (deviceModelId.HasValue)
            query = query.Where(s => s.DeviceModelId == deviceModelId.Value);

        query = sortOrder switch
        {
            1 => query.OrderByDescending(s => s.Name),
            2 => query.OrderBy(s => s.Price),
            3 => query.OrderByDescending(s => s.Price),
            _ => query.OrderBy(s => s.Name),
        };
        return query;
    }

    public async Task<List<Service>> GetFilteredForManagerAsync(string? searchTerm, bool? status, Guid? categoryId,
        Guid? deviceTypeId, Guid? manufacturerId, Guid? deviceModelId, int sortOrder,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Services
            .Include(s => s.Category)
            .Include(s => s.DeviceType)
            .Include(s => s.DeviceModel)
            .Include(s => s.Manufacturer)
            .Where(s => !s.IsDeleted)
            .AsQueryable();

        return await ApplyFilters(query, searchTerm, categoryId, deviceTypeId, manufacturerId, deviceModelId, status, sortOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Service>> GetFilteredForClientAsync(string? searchTerm, Guid? categoryId,
        Guid? deviceTypeId, Guid? manufacturerId, Guid? deviceModelId, int sortOrder,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Services
            .Include(s => s.Category)
            .Include(s => s.DeviceType)
            .Include(s => s.DeviceModel)
            .Include(s => s.Manufacturer)
            .Where(s => s.IsAvailable && !s.IsDeleted)
            .AsQueryable();

        return await ApplyFilters(query, searchTerm, categoryId, deviceTypeId, manufacturerId, deviceModelId, null, sortOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsDuplicateAsync(Guid? id, string name, Guid deviceTypeId, Guid? deviceModelId,
        CancellationToken ct)
    {
        return await _context.Services
            .AnyAsync(s =>
                    s.Id != id &&
                    s.Name == name &&
                    s.DeviceTypeId == deviceTypeId &&
                    s.DeviceModelId == deviceModelId &&
                    !s.IsDeleted,
                ct);
    }

    public async Task<bool> HasLinkedRequestsAsync(Guid serviceId, CancellationToken ct)
    {
        return await _context.RequestServices.AnyAsync(rs => rs.ServiceId == serviceId, ct);
    }

    public async Task Delete(Service service, CancellationToken ct)
    {
        bool hasLinks = await HasLinkedRequestsAsync(service.Id, ct);

        if (hasLinks)
        {
            service.Archive();
            _context.Services.Update(service);
        }
        else
        {
            _context.Services.Remove(service);
        }
    }
}