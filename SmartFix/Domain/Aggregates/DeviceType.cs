namespace SmartFix.Domain.Aggregates;

public class DeviceType
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    private DeviceType()
    {
    }

    public static DeviceType Create(string name) => new DeviceType { Name = name };
    public void Update(string newName) => Name = newName;
}