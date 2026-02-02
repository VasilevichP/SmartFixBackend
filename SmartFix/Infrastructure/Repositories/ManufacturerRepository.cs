using Microsoft.EntityFrameworkCore;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Infrastructure.Persistence;

namespace SmartFix.Infrastructure.Repositories;

public class ManufacturerRepository: IManufacturerRepository
{
    private readonly AppDbContext _context;

    public ManufacturerRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Manufacturer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Manufacturers.FirstOrDefaultAsync(s=>s.Id == id, cancellationToken);
    }

    public async Task<List<Manufacturer>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Manufacturers.OrderBy(s => s.Name).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Manufacturer manufacturer, CancellationToken cancellationToken = default)
    {
        await _context.Manufacturers.AddAsync(manufacturer, cancellationToken);
    }

    public void Update(Manufacturer manufacturer)
    {
        _context.Manufacturers.Update(manufacturer);
    }

    public void Delete(Manufacturer manufacturer)
    {
        _context.Manufacturers.Remove(manufacturer);
    }
}