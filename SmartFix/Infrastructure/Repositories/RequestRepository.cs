using Microsoft.EntityFrameworkCore;
using SmartFix.Application.Features.Requests.DTO;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Domain.ValueObjects;
using SmartFix.Infrastructure.Persistence;

namespace SmartFix.Infrastructure.Repositories;

public class RequestRepository : IRequestRepository
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
            .AsNoTracking()
            .Include(r => r.Services)
            .Include(r => r.Client)
            .Include(r=>r.AppliedDiscounts)
            .Include(r => r.DeviceType)
            .Include(r => r.Master)
            .Include(r => r.StatusHistories)
            .Include(r => r.Photos)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<List<Request>> GetAllAsync(string? client,string? device, RequestStatus? status,
        int sortOrder, CancellationToken cancellationToken = default)
    {
        var query = _context.Requests
            .AsNoTracking()
            .Include(r => r.Client)
            .Include(r => r.Services)
            .Include(r => r.Master)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(client))
        {
            query = query.Where(r => 
                r.Client.Name.ToLower().Contains(client)
            );
        }
        if (!string.IsNullOrWhiteSpace(device))
        {
            query = query.Where(r => 
                r.DeviceModelName.ToLower().Contains(device)
            );
        }

        if (status.HasValue)
        {
            query = query.Where(r => r.Status == status.Value);
        }

        query = sortOrder switch
        {
            1 => query.OrderBy(r => r.CreatedAt),           
            _ => query.OrderByDescending(r => r.CreatedAt), 
        };

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<List<Request>> GetAllForClientAsync(Guid clientId, CancellationToken cancellationToken = default)
    {
        return await _context.Requests
            .AsNoTracking()
            .Include(r => r.Services)
            .Include(r => r.AppliedDiscounts)
            .OrderByDescending(r => r.CreatedAt)
            .Where(r => r.ClientId == clientId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public void Update(Request request)
    {
        _context.Requests.Update(request);
    }
}