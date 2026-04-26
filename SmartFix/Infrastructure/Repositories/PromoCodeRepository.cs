using Microsoft.EntityFrameworkCore;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Infrastructure.Persistence;

namespace SmartFix.Infrastructure.Repositories;

public class PromoCodeRepository: IPromoCodeRepository
{
    private readonly AppDbContext _context;

    public PromoCodeRepository(AppDbContext context) => _context = context;

    public async Task AddAsync(PromoCode promoCode, CancellationToken cancellationToken = default)
    {
        await _context.PromoCodes.AddAsync(promoCode, cancellationToken);
    }

    public async Task<PromoCode?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.PromoCodes.SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<PromoCode?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _context.PromoCodes
            .SingleOrDefaultAsync(p => p.Code.ToLower() == code.ToLower(), cancellationToken);
    }

    public async Task<List<PromoCode>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.PromoCodes
            .OrderBy(p => p.ExpirationDate)
            .ToListAsync(cancellationToken);
    }

    public void Update(PromoCode promoCode)
    {
        _context.PromoCodes.Update(promoCode);
    }
}