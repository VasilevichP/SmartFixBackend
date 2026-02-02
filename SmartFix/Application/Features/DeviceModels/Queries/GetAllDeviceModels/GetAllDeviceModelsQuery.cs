using MediatR;
using SmartFix.Application.Features.DeviceModels.DTO;

namespace SmartFix.Application.Features.DeviceModels.Queries.GetAllDeviceModels;

public class GetAllDeviceModelsQuery: IRequest<List<DeviceModelDto>>
{
    public Guid? DeviceTypeId { get; set; }  
    public Guid? ManufacturerId { get; set; } 
}