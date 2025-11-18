using Microsoft.EntityFrameworkCore;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Infrastructure.Persistence;

namespace SmartFix.Infrastructure.Repositories;

public class ServiceCategoryRepository : IServiceCategoryRepository
{
    private readonly AppDbContext _context;

    public ServiceCategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceCategory?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ServiceCategories.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<List<ServiceCategory>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ServiceCategories.OrderBy(c => c.Name).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(ServiceCategory category, CancellationToken cancellationToken = default)
    {
        await _context.ServiceCategories.AddAsync(category, cancellationToken);
    }

    public void Update(ServiceCategory category)
    {
        _context.ServiceCategories.Update(category);
    }

    public void Delete(ServiceCategory category)
    {
        _context.ServiceCategories.Remove(category);
    }
}