using Microsoft.EntityFrameworkCore;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Infrastructure.Persistence;

namespace SmartFix.Infrastructure.Repositories;

public class ReviewRepository: IReviewRepository
{
    private readonly AppDbContext _context;

    public ReviewRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Review review, CancellationToken cancellationToken = default)
    {
        await _context.Reviews.AddAsync(review, cancellationToken);
    }

    public async Task<Review?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Reviews.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<List<Review>> GetAllByServiceAsync(Guid serviceId, CancellationToken cancellationToken = default)
    {
        return await _context.Reviews
            .Where(r => r.ServiceId == serviceId)
            .ToListAsync(cancellationToken);
    }

    public void Delete(Review review)
    {
        _context.Reviews.Remove(review);
    }
}