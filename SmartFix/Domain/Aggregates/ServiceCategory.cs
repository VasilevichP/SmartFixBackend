namespace SmartFix.Domain.Aggregates;

public class ServiceCategory
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    // Навигационное свойство для EF Core
    private readonly List<Service> _services = new();
    public IReadOnlyCollection<Service> Services => _services.AsReadOnly();

    // Приватный конструктор для EF Core
    private ServiceCategory()
    {
    }

    // Фабричный метод для создания категории
    public static ServiceCategory Create(string name)
    {
        // Здесь можно добавить валидацию, например, на пустую строку
        return new ServiceCategory
        {
            Id = Guid.NewGuid(),
            Name = name
        };
    }
}