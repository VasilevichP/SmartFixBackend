using MediatR;
using SmartFix.Application.Features.Services.DTO;

namespace SmartFix.Application.Features.Services.Queries.GetAllForClient;

public class GetAllServicesForClientQuery : IRequest<List<ServiceDTO>>
{
    public string? SearchTerm { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? DeviceTypeId { get; set; }  
    public Guid? ManufacturerId { get; set; }  
    public Guid? DeviceModelId { get; set; }
    public int SortOrder { get; set; }
}