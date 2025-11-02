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
        return await _context.Services.FindAsync(new object[] { id }, cancellationToken);
    }

    public void Update(Service service)
    {
        _context.Services.Update(service);
    }
    
    public async Task<List<Service>> GetAllForClientAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Services
            .Include(s => s.Category)
            .Where(s => s.IsAvailable)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Service>> GetAllForManagerAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Services
            .Include(s => s.Category)
            .ToListAsync(cancellationToken);
    }
}