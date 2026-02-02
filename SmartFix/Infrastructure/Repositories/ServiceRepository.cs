using Microsoft.EntityFrameworkCore;
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
            .Include(s => s.Reviews)
            .ThenInclude(r => r.Client)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public void Update(Service service)
    {
        _context.Services.Update(service);
    }

    // public async Task<List<Service>> GetAllForClientAsync(CancellationToken cancellationToken = default)
    // {
    //     return await _context.Services
    //         .Include(s => s.Category)
    //         .Where(s => s.IsAvailable)
    //         .ToListAsync(cancellationToken);
    // }
    //
    // public async Task<List<Service>> GetAllForManagerAsync(CancellationToken cancellationToken = default)
    // {
    //     return await _context.Services
    //         .Include(s => s.Category)
    //         .Include(s=>s.DeviceType)
    //         .Include(s => s.DeviceModel)
    //         .Include(s=>s.Manufacturer)
    //         .ToListAsync(cancellationToken);
    // }

    public async Task<List<Service>> GetFilteredAsync(string? searchTerm, bool? status, Guid? categoryId,
        Guid? deviceTypeId, Guid? manufacturerId, Guid? deviceModelId, int sortOrder,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Services
            .Include(s => s.Category)
            .Include(s=>s.DeviceType)
            .Include(s => s.DeviceModel)
            .Include(s=>s.Manufacturer)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(s => s.Name.ToLower().Contains(searchTerm) ||
                                     s.Category.Name.Contains(searchTerm));
        }

        if (status.HasValue)
            query = query.Where(s => s.IsAvailable == status.Value);

        if (categoryId.HasValue)
            query = query.Where(s => s.CategoryId == categoryId.Value);

        if (deviceTypeId.HasValue)
            query = query.Where(s => s.DeviceTypeId == deviceTypeId.Value);
        
        if (manufacturerId.HasValue)
            query = query.Where(s => 
                s.ManufacturerId == manufacturerId.Value || 
                (s.DeviceModel != null && s.DeviceModel.ManufacturerId == manufacturerId.Value)
            );;

        if (deviceModelId.HasValue)
            query = query.Where(s => s.DeviceModelId == deviceModelId.Value);

        query = sortOrder switch
        {
            1 => query.OrderByDescending(s => s.Name),
            2 => query.OrderBy(s => s.Price),
            3 => query.OrderByDescending(s => s.Price),
            _ => query.OrderBy(s => s.Name),
        };

        return await query.ToListAsync(cancellationToken);
    }

    public void Delete(Service service)
    {
        _context.Services.Remove(service);
    }
}