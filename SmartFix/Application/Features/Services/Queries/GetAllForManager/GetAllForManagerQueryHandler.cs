using MediatR;
using SmartFix.Application.Features.Services.DTO;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Services.Queries.GetAllForManager;

public class GetAllForManagerQueryHandler : IRequestHandler<GetAllForManagerQuery, List<ServiceDTO>>
{
    private readonly IServiceRepository _serviceRepository;

    public GetAllForManagerQueryHandler(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<List<ServiceDTO>> Handle(GetAllForManagerQuery request, CancellationToken cancellationToken)
    {
        var services = await _serviceRepository.GetAllForManagerAsync(cancellationToken);

        return services.Select(s => new ServiceDTO()
        {
            Id = s.Id,
            Name = s.Name,
            Price = s.Price,
            CategoryName = s.Category.Name,
            IsAvailable = s.IsAvailable
        }).ToList();
    }
}