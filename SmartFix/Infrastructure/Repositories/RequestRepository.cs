using Microsoft.EntityFrameworkCore;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Infrastructure.Persistence;

namespace SmartFix.Infrastructure.Repositories;

public class RequestRepository:IRequestRepository
{
    private readonly AppDbContext _context;
    public RequestRepository(AppDbContext context) => _context = context;
    public async Task AddAsync(Request request, CancellationToken cancellationToken = default)
    {
        await _context.Requests.AddAsync(request, cancellationToken);
    }

    public async Task<Request?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Requests
            .Include(r => r.Service)
            .Include(r => r.Client)
            .Include(r => r.StatusHistories)
            .Include(r => r.Photos) 
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<List<Request>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Requests
            .Include(r => r.Service)
            .Include(r => r.Client)
            .Include(r=>r.Specialist)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<List<Request>> GetAllForClientAsync(Guid clientId, CancellationToken cancellationToken = default)
    {
        return await _context.Requests
            .Include(r => r.Service)
            .OrderByDescending(r => r.CreatedAt)
            .Where(r=>r.ClientId == clientId)
            .ToListAsync(cancellationToken);
    }
    public void Update(Request request)
         {
             _context.Requests.Update(request);
         }

}