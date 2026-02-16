using Microsoft.EntityFrameworkCore;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Infrastructure.Persistence;

namespace SmartFix.Infrastructure.Repositories;

public class DeviceModelRepository : IDeviceModelRepository
{
    private readonly AppDbContext _context;

    public DeviceModelRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DeviceModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.DeviceModels
            .Include(s => s.DeviceType)
            .Include(s => s.Manufacturer)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<List<DeviceModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.DeviceModels
            .Include(s => s.Manufacturer)
            .Include(s => s.DeviceType)
            .Where(s => !s.IsDeleted)
            .OrderBy(s => s.Name).ToListAsync(cancellationToken);
    }

    public async Task<List<DeviceModel>> GetAllByTypeAndManufacturerAsync(Guid? typeId, Guid? manufacturerId,
        CancellationToken cancellationToken = default)
    {
        var query = _context.DeviceModels
            .Include(s => s.DeviceType)
            .Include(s => s.Manufacturer)
            .Where(s => !s.IsDeleted)
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

    public async Task<bool> IsUsedInSystemAsync(Guid id, CancellationToken ct)
    {
        bool inRequests = await _context.Requests.AnyAsync(r => r.DeviceModelId == id, ct);
        if (inRequests) return true;

        bool inServices = await _context.Services.AnyAsync(s => s.DeviceModelId == id, ct);
        return inServices;
    }

    public async Task<bool> ExistsByNameAndManufacturer(string name, Guid manufacturerId, CancellationToken ct)
        => await _context.DeviceModels.AnyAsync(d => d.Name == name && d.ManufacturerId == manufacturerId, ct);

    public async Task Delete(DeviceModel deviceModel, CancellationToken ct)
    {
        bool usedInRequests = await _context.Requests.AnyAsync(r => r.DeviceModelId == deviceModel.Id, ct);

        bool usedInServices = await _context.Services.AnyAsync(s => s.DeviceModelId == deviceModel.Id, ct);

        if (usedInRequests || usedInServices)
        {
            deviceModel.Archive();
            _context.DeviceModels.Update(deviceModel);
        }
        else
        {
            _context.DeviceModels.Remove(deviceModel);
        }
    }
}