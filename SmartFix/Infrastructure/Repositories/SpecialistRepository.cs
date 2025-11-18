using Microsoft.EntityFrameworkCore;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
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
        return await _context.Specialists.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<List<Specialist>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Specialists.OrderBy(s => s.FullName).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Specialist specialist, CancellationToken cancellationToken = default)
    {
        await _context.Specialists.AddAsync(specialist, cancellationToken);
    }

    public void Update(Specialist specialist)
    {
        _context.Specialists.Update(specialist);
    }

    public void Delete(Specialist specialist)
    {
        _context.Specialists.Remove(specialist);
    }
}