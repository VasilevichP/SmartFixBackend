using SmartFix.Domain.Aggregates;

namespace SmartFix.Domain.Abstractions;

public interface IReviewRepository
{
    Task AddAsync(Review review, CancellationToken cancellationToken = default);
    Task<Review?> GetByRequestIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByRequest(Guid requestId, CancellationToken cancellationToken = default);
    void Delete(Review review);
}