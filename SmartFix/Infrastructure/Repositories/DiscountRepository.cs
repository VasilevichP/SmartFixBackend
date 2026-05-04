using Microsoft.EntityFrameworkCore;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Infrastructure.Persistence;

namespace SmartFix.Infrastructure.Repositories;

public class DiscountRepository:IDiscountRepository
{
    private readonly AppDbContext _context;

    public DiscountRepository(AppDbContext context) => _context = context;

    public async Task AddAsync(Discount rule, CancellationToken cancellationToken = default)
    {
        await _context.Discounts.AddAsync(rule, cancellationToken);
    }

    public async Task<Discount?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Discounts.SingleOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<List<Discount>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Discounts
            .OrderByDescending(d => d.Priority)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Discount>> GetAllActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Discounts
            .Where(d => d.IsActive)
            .OrderByDescending(d => d.Priority)
            .ToListAsync(cancellationToken);
    }

    public void Update(Discount rule)
    {
        _context.Discounts.Update(rule);
    }
}