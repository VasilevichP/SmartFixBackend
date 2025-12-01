using Microsoft.EntityFrameworkCore;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Infrastructure.Persistence;

namespace SmartFix.Infrastructure.Repositories;

public class DeviceTypeRepository: IDeviceTypeRepository
{
    private readonly AppDbContext _context;

    public DeviceTypeRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<DeviceType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.DeviceTypes.FirstOrDefaultAsync(d=>d.Id == id, cancellationToken);
    }

    public async Task<List<DeviceType>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.DeviceTypes.OrderBy(d => d.Name).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(DeviceType deviceType, CancellationToken cancellationToken = default)
    {
        await _context.DeviceTypes.AddAsync(deviceType, cancellationToken);
    }

    public void Update(DeviceType deviceType)
    {
        _context.DeviceTypes.Update(deviceType);
    }

    public void Delete(DeviceType deviceType)
    {
        _context.DeviceTypes.Remove(deviceType);
    }
}