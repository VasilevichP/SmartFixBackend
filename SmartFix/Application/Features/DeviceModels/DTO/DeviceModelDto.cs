namespace SmartFix.Application.Features.DeviceModels.DTO;

public class DeviceModelDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid ManufacturerId { get; set; }
    public Guid DeviceTypeId { get; set; }
}