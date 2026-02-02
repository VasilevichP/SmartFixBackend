using Microsoft.EntityFrameworkCore;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Infrastructure.Persistence;

namespace SmartFix.Infrastructure.Repositories;

public class DeviceModelRepository: IDeviceModelRepository
{
    private readonly AppDbContext _context;

    public DeviceModelRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<DeviceModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.DeviceModels
            .Include(s=>s.DeviceType)
            .Include(s=>s.Manufacturer)
            .FirstOrDefaultAsync(s=>s.Id == id, cancellationToken);
    }

    public async Task<List<DeviceModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.DeviceModels
            .Include(s=>s.Manufacturer)
            .Include(s=>s.DeviceType)
            .OrderBy(s => s.Name).ToListAsync(cancellationToken);
    }

    public async Task<List<DeviceModel>> GetAllByTypeAndManufacturerAsync(Guid? typeId, Guid? manufacturerId,
        CancellationToken cancellationToken = default)
    {
        var query = _context.DeviceModels
            .AsQueryable();
        
        if (typeId.HasValue)
            query = query.Where(s => s.DeviceTypeId == typeId.Value);
        if (manufacturerId.HasValue)
            query = query.Where(s => s.ManufacturerId == manufacturerId.Value);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(DeviceModel deviceModel, CancellationToken cancellationToken = default)
    {
        await _context.DeviceModels.AddAsync(deviceModel, cancellationToken);
    }

    public void Update(DeviceModel deviceModel)
    {
        _context.DeviceModels.Update(deviceModel);
    }

    public void Delete(DeviceModel deviceModel)
    {
        _context.DeviceModels.Remove(deviceModel);
    }
}