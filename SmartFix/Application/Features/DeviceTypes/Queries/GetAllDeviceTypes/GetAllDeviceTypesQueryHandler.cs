using MediatR;
using SmartFix.Application.Features.DeviceTypes.DTO;
using SmartFix.Application.Features.Specialists.DTO;
using SmartFix.Application.Features.Specialists.Queries.GetAll;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Application.Features.DeviceTypes.Queries.GetAllDeviceTypes;

public class GetAllDeviceTypesQueryHandler: IRequestHandler<GetAllDeviceTypesQuery, List<DeviceTypeDto>>
{
    private readonly IDeviceTypeRepository _deviceTypeRepository;

    public GetAllDeviceTypesQueryHandler(IDeviceTypeRepository deviceTypeRepository)
    {
        _deviceTypeRepository = deviceTypeRepository;
    }

    public async Task<List<DeviceTypeDto>> Handle(GetAllDeviceTypesQuery request, CancellationToken cancellationToken)
    {
        var deviceTypes = await _deviceTypeRepository.GetAllAsync(cancellationToken);
        return deviceTypes
            .Select(s => new DeviceTypeDto { Id = s.Id, Name = s.Name })
            .ToList();
    }
}