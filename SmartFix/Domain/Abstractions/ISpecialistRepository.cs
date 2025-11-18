using SmartFix.Domain.Aggregates;

namespace SmartFix.Domain.Abstractions;

public interface ISpecialistRepository
{
    Task<Specialist?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Specialist>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Specialist specialist, CancellationToken cancellationToken = default);
    void Update(Specialist specialist);
    void Delete(Specialist specialist);
}