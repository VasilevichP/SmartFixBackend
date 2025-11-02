namespace SmartFix.Domain.Aggregates;

public class Service
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public string? WarrantyPeriod { get; private set; }
    public bool IsAvailable { get; private set; }
    public string? PhotoName { get; private set; }
    public string? PhotoPath { get; private set; }
    
    public Guid CategoryId { get; private set; }
    public ServiceCategory Category { get; private set; }

   private Service() { }

    public static Service Create(string name, decimal price, Guid categoryId, string photoName, string photoPath, string? description = null, string? warrantyPeriod = null)
    {
        // Здесь можно добавить валидацию (цена > 0, имя не пустое и т.д.)
        return new Service
        {
            Id = Guid.NewGuid(),
            Name = name,
            Price = price,
            CategoryId = categoryId,
            PhotoName = photoName,
            PhotoPath = photoPath,
            Description = description,
            WarrantyPeriod = warrantyPeriod,
            IsAvailable = true
        };
    }

    public void UpdateDetails(string name, decimal price, string? description, string? warrantyPeriod)
    {
        Name = name;
        Price = price;
        Description = description;
        WarrantyPeriod = warrantyPeriod;
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