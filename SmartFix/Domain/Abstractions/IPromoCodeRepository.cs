using SmartFix.Domain.Aggregates;

namespace SmartFix.Domain.Abstractions;

public interface IPromoCodeRepository
{
    Task AddAsync(PromoCode promoCode, CancellationToken cancellationToken = default);
    Task<PromoCode?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PromoCode?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<List<PromoCode>> GetAllAsync(CancellationToken cancellationToken = default);
    void Update(PromoCode promoCode);
}