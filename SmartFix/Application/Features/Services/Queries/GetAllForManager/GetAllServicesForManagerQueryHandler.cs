using MediatR;
using SmartFix.Application.Features.Services.DTO;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.Services.Queries.GetAllForManager;

public class GetAllServicesForManagerQueryHandler : IRequestHandler<GetAllServicesForManagerQuery, List<ServiceDTO>>
{
    private readonly IServiceRepository _serviceRepository;

    public GetAllServicesForManagerQueryHandler(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<List<ServiceDTO>> Handle(GetAllServicesForManagerQuery request, CancellationToken cancellationToken)
    {
        var services = await _serviceRepository.GetFilteredAsync(
            request.SearchTerm,
            request.Status,
            request.CategoryId,
            request.DeviceTypeId,
            request.ManufacturerId,
            request.DeviceModelId,
            request.SortOrder,
            cancellationToken
        );

        return services.Select(s => new ServiceDTO()
        {
            Id = s.Id,
            Name = s.Name,
            Price = s.Price,
            CategoryId = s.CategoryId,
            CategoryName = s.Category.Name,
            DeviceTypeId = s.DeviceTypeId,
            DeviceTypeName = s.DeviceType.Name,
            ManufacturerId = s.ManufacturerId,
            ManufacturerName = s.Manufacturer?.Name,
            DeviceModelId = s.DeviceModelId,
            DeviceModelName = s.DeviceModel?.Name,
            IsAvailable = s.IsAvailable
        }).ToList();
    }
}