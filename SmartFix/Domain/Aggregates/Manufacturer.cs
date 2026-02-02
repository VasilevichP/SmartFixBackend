namespace SmartFix.Domain.Aggregates;

public class Manufacturer
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    private Manufacturer() { }

    public static Manufacturer Create(string name)
    {
        return new Manufacturer { Name = name };
    }
    
    public void Update(string newName) => Name = newName;
}