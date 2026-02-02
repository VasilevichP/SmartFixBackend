namespace SmartFix.Domain.Aggregates;

public class Service
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public int WarrantyPeriod { get; private set; }
    public bool IsAvailable { get; private set; }
    public Guid CategoryId { get; private set; }
    public ServiceCategory Category { get; private set; }

    public Guid DeviceTypeId { get; private set; }
    public DeviceType DeviceType { get; private set; }
    public Guid? ManufacturerId { get; private set; }
    public Manufacturer? Manufacturer { get; private set; }
    public Guid? DeviceModelId { get; private set; }
    public DeviceModel? DeviceModel { get; private set; }

    private readonly List<Review> _reviews = new();
    public IReadOnlyCollection<Review> Reviews => _reviews.AsReadOnly();

    private Service()
    {
    }

    public static Service Create(string name, decimal price, Guid categoryId, string? description,
        int warrantyPeriod, Guid deviceTypeId, Guid? deviceModelId, Guid? manufacturerId)
    {
        // Здесь можно добавить валидацию (цена > 0, имя не пустое и т.д.)
        return new Service
        {
            Id = Guid.NewGuid(),
            Name = name,
            Price = price,
            CategoryId = categoryId,
            DeviceTypeId = deviceTypeId,
            DeviceModelId = deviceModelId,
            ManufacturerId = manufacturerId,
            Description = description,
            WarrantyPeriod = warrantyPeriod,
            IsAvailable = true
        };
    }

    public void UpdateDetails(string name, decimal price, string? description, int warrantyPeriod, Boolean isAvailable,
        Guid categoryId, Guid deviceTypeId, Guid? manufacturerId, Guid? deviceModelId)
    {
        Name = name;
        Price = price;
        Description = description;
        WarrantyPeriod = warrantyPeriod;
        CategoryId = categoryId;
        DeviceTypeId = deviceTypeId;
        ManufacturerId = manufacturerId;
        DeviceModelId = deviceModelId;
        IsAvailable = isAvailable;
    }

    public void Hide()
    {
        IsAvailable = false;
    }

    public void Show()
    {
        IsAvailable = true;
    }
}