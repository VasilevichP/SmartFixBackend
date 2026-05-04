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

    public async Task<Review?> GetByRequestIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Reviews.FirstOrDefaultAsync(r=>r.RequestId == id, cancellationToken);
    }

    public async Task<bool> ExistsByRequest(Guid requestId, CancellationToken cancellationToken = default)
    {
        return await _context.Reviews.AnyAsync(x => x.RequestId == requestId, cancellationToken);
    }

    public void Delete(Review review)
    {
        _context.Reviews.Remove(review);
    }
}