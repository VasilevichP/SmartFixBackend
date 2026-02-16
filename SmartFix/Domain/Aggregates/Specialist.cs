namespace SmartFix.Domain.Aggregates;

public class Specialist
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    private Specialist() { }
    
    public static Specialist Create(string fullName)
    {
        return new Specialist
        {
            Name = fullName
        };
    }
    
    public void Update(string newFullName)
    {
        Name = newFullName;
    }
}