namespace SmartFix.Domain.Aggregates;

public class DeviceModel
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    
    public Guid ManufacturerId { get; private set; }
    public Manufacturer Manufacturer { get; private set; }

    public Guid DeviceTypeId { get; private set; }
    public DeviceType DeviceType { get; private set; }

    private DeviceModel() { }

    public static DeviceModel Create(string name, Guid manufacturerId, Guid deviceTypeId)
    {
        return new DeviceModel
        {
            Name = name,
            ManufacturerId = manufacturerId,
            DeviceTypeId = deviceTypeId
        };
    }
    public void Update(string name, Guid manufacturerId, Guid deviceTypeId)
    {
        Name = name;
        ManufacturerId = manufacturerId;
        DeviceTypeId = deviceTypeId;
    } 
}