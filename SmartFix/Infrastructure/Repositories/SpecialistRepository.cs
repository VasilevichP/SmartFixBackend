using Microsoft.EntityFrameworkCore;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Domain.ValueObjects;
using SmartFix.Infrastructure.Persistence;

namespace SmartFix.Infrastructure.Repositories;

public class SpecialistRepository : ISpecialistRepository
{
    private readonly AppDbContext _context;

    public SpecialistRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Specialist?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Specialists.FirstOrDefaultAsync(s=>s.Id == id, cancellationToken);
    }

    public async Task<List<Specialist>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Specialists.OrderBy(s => s.Name).ToListAsync(cancellationToken);
    }

    public async Task<List<(Specialist Specialist, int Load)>> GetAllWithLoadAsync(CancellationToken cancellationToken = default)
    {
        var query = _context.Specialists
            .Select(s => new
            {
                Specialist = s,
                Load = _context.Requests.Count(r => 
                    r.SpecialistId == s.Id && 
                    r.Status != RequestStatus.Closed && 
                    r.Status != RequestStatus.Cancelled)
            });

        var result = await query.ToListAsync(cancellationToken);

        return result.Select(x => (x.Specialist, x.Load)).ToList();
    }

    public async Task AddAsync(Specialist specialist, CancellationToken cancellationToken = default)
    {
        await _context.Specialists.AddAsync(specialist, cancellationToken);
    }
    
    public async Task<bool> ExistsByName(string name, CancellationToken ct)
        => await _context.Specialists.AnyAsync(d => d.Name == name,ct);

    public async Task<bool> HasRelatedRequestsAsync(Guid id, CancellationToken ct)
        => await _context.Requests.AnyAsync(r => r.SpecialistId == id, ct);

    public void Update(Specialist specialist)
    {
        _context.Specialists.Update(specialist);
    }

    public void Delete(Specialist specialist)
    {
        _context.Specialists.Remove(specialist);
    }
}