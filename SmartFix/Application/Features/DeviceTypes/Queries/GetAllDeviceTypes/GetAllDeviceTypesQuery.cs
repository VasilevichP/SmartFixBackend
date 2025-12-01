using MediatR;
using SmartFix.Application.Features.DeviceTypes.DTO;

namespace SmartFix.Application.Features.DeviceTypes.Queries.GetAllDeviceTypes;

public class GetAllDeviceTypesQuery: IRequest<List<DeviceTypeDto>>
{
}