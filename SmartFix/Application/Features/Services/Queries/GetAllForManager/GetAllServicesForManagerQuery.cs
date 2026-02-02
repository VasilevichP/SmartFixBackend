using MediatR;
using SmartFix.Application.Features.Services.DTO;

namespace SmartFix.Application.Features.Services.Queries.GetAllForManager;

public class GetAllServicesForManagerQuery : IRequest<List<ServiceDTO>>
{
    public string? SearchTerm { get; set; }
    public bool? Status { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? DeviceTypeId { get; set; }  
    public Guid? ManufacturerId { get; set; }  
    public Guid? DeviceModelId { get; set; }
    public int SortOrder { get; set; }
}