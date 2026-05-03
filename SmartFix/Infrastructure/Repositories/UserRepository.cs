using Microsoft.EntityFrameworkCore;
using SmartFix.Application.Features.Masters.DTO;
using SmartFix.Domain.Abstractions;
using SmartFix.Domain.Aggregates;
using SmartFix.Domain.ValueObjects;
using SmartFix.Infrastructure.Persistence;

namespace SmartFix.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Users.SingleOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<Client?> GetClientByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Clients.SingleOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Master?> GetMasterByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Masters.SingleOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<bool> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users.AnyAsync(u => u.Email == email, cancellationToken);
    }

    public void Update(User user, CancellationToken cancellationToken = default)
    {
        _context.Users.Update(user);
    }

    public async Task<List<Client>> GetClientsListAsync(string? nameSearch, string? phoneSearch, ClientStatus? status,
        int sortOrder,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Clients.AsQueryable();

        if (!string.IsNullOrWhiteSpace(nameSearch))
        {
            var search = nameSearch.ToLower();
            query = query.Where(c =>
                (c.Name != null && c.Name.ToLower().Contains(search)));
        }

        if (!string.IsNullOrWhiteSpace(phoneSearch))
        {
            var search = phoneSearch.ToLower();
            query = query.Where(c =>
                (c.PhoneNumber != null && c.PhoneNumber.ToLower().Contains(phoneSearch)));
        }

        if (status.HasValue)
        {
            query = query.Where(c => c.Status == status.Value);
        }

        query = sortOrder switch
        {
            2 => query.OrderByDescending(c => c.Name),
            _ => query.OrderBy(c => c.Name),
        };

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<List<Master>> GetMasterListAsync(string? nameSearch, string? phoneSearch,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Masters.AsQueryable().Where(m => !m.IsDeleted);

        if (!string.IsNullOrWhiteSpace(nameSearch))
        {
            var search = nameSearch.ToLower();
            query = query.Where(c =>
                (c.Name != null && c.Name.ToLower().Contains(search)));
        }

        if (!string.IsNullOrWhiteSpace(phoneSearch))
        {
            var search = phoneSearch.ToLower();
            query = query.Where(c =>
                (c.Name != null && c.PhoneNumber.ToLower().Contains(search)));
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<List<MasterSelectDto>> GetAllMastersForSelect(CancellationToken cancellationToken)
    {
        var masters = await _context.Masters
            .AsNoTracking()
            .Where(m => !m.IsDeleted)
            .ToListAsync(cancellationToken);
        var dtos = new List<MasterSelectDto>();
        foreach (var master in masters)
        {
            var requests = await CountMasterActiveRequestsAsync(master.Id, cancellationToken);
            dtos.Add(new MasterSelectDto{Id = master.Id, Name = master.Name, ActiveRequestsCount = requests});
        }

        return dtos;
    }

    public async Task<int> CountMasterActiveRequestsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Requests.CountAsync(
            c => c.MasterId == id && !(c.Status == RequestStatus.Cancelled || c.Status == RequestStatus.Closed),
            cancellationToken);
    }

    public async Task<int> GetClientOrdersCountAsync(Guid clientId, CancellationToken ct)
    {
        return await _context.Requests.CountAsync(c => c.ClientId == clientId, ct);
    }
}