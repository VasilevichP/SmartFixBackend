using MediatR;
using SmartFix.Application.Features.Services.DTO;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Services.Queries.GetAllForClient;

public class GetAllServicesForClientQueryHandler : IRequestHandler<GetAllServicesForClientQuery, List<ServiceDTOForClient>>
{
    private readonly IServiceRepository _serviceRepository;

    public GetAllServicesForClientQueryHandler(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<List<ServiceDTOForClient>> Handle(GetAllServicesForClientQuery request, CancellationToken cancellationToken)
    {
        var services = await _serviceRepository.GetFilteredForClientAsync(
            request.SearchTerm,
            request.CategoryId,
            request.DeviceTypeId,
            request.ManufacturerId,
            request.DeviceModelId,
            request.SortOrder,
            cancellationToken
        );

        return services.Select(s => new ServiceDTOForClient
        {
            Id = s.Id,
            Name = s.Name,
            Price = s.Price,
            CategoryName = s.Category.Name,
            DeviceTypeId = s.DeviceTypeId,
            DeviceTypeName = s.DeviceType.Name,
            ManufacturerId = s.ManufacturerId,
            ManufacturerName = s.Manufacturer?.Name,
            DeviceModelId = s.DeviceModelId,
            DeviceModelName = s.DeviceModel?.Name,
            AverageRating = s.Reviews.Any() 
                ? Math.Round(s.Reviews.Average(r => r.Rating), 1) 
                : 0
        }).ToList();
    }
}