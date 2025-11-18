namespace SmartFix.Domain.Aggregates;

public class ServiceCategory
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    private readonly List<Service> _services = new();
    public IReadOnlyCollection<Service> Services => _services.AsReadOnly();
    
    private ServiceCategory() { }

    public static ServiceCategory Create(string name)
    {
        // TODO: Добавить валидацию (например, имя не должно быть пустым)
        return new ServiceCategory
        {
            Name = name
        };
    }
    
    public void UpdateName(string newName)
    {
        // TODO: Добавить валидацию
        Name = newName;
    }
}