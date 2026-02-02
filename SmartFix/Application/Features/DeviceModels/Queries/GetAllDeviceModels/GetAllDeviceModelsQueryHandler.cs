using MediatR;
using SmartFix.Application.Features.DeviceModels.DTO;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.DeviceModels.Queries.GetAllDeviceModels;

public class GetAllDeviceModelsQueryHandler : IRequestHandler<GetAllDeviceModelsQuery, List<DeviceModelDto>>
{
    private readonly IDeviceModelRepository _deviceModelRepository;

    public GetAllDeviceModelsQueryHandler(IDeviceModelRepository deviceModelRepository)
    {
        _deviceModelRepository = deviceModelRepository;
    }

    public async Task<List<DeviceModelDto>> Handle(GetAllDeviceModelsQuery request, CancellationToken cancellationToken)
    {
        var categories = await _deviceModelRepository.GetAllByTypeAndManufacturerAsync(
            request.DeviceTypeId, request.ManufacturerId, cancellationToken);

        return categories
            .Select(d => new DeviceModelDto
                { Id = d.Id, Name = d.Name, DeviceTypeId = d.DeviceTypeId, ManufacturerId = d.ManufacturerId })
            .ToList();
    }
}