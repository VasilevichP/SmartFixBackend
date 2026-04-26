using SmartFix.Domain.Aggregates;

namespace SmartFix.Domain.Abstractions;

public interface IDiscountRepository
{
    Task AddAsync(Discount rule, CancellationToken cancellationToken = default);
    Task<Discount?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Discount>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<Discount>> GetAllActiveAsync(CancellationToken cancellationToken = default);
    void Update(Discount rule);
}