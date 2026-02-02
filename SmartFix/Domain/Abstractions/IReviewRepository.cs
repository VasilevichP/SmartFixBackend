using SmartFix.Domain.Aggregates;

namespace SmartFix.Domain.Abstractions;

public interface IReviewRepository
{
    Task AddAsync(Review review, CancellationToken cancellationToken = default);
    Task<Review?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Review>> GetAllByServiceAsync(Guid serviceId, CancellationToken cancellationToken = default);
    void Delete(Review review);
}