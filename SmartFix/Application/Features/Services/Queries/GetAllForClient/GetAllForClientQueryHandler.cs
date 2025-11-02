using MediatR;
using SmartFix.Application.Features.Services.DTO;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Services.Queries.GetAllForClient;

public class GetAllForClientQueryHandler : IRequestHandler<GetAllForClientQuery, List<ServiceDTO>>
{
    private readonly IServiceRepository _serviceRepository;

    public GetAllForClientQueryHandler(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<List<ServiceDTO>> Handle(GetAllForClientQuery request, CancellationToken cancellationToken)
    {
        var services = await _serviceRepository.GetAllForClientAsync(cancellationToken);

        return services.Select(s => new ServiceDTO
        {
            Id = s.Id,
            Name = s.Name,
            Price = s.Price,
            CategoryName = s.Category.Name,
            IsAvailable = s.IsAvailable
        }).ToList();
    }
}