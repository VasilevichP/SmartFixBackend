namespace SmartFix.Domain.Aggregates;

public class Specialist
{
    public Guid Id { get; private set; }
    public string FullName { get; private set; }
    private Specialist() { }
    
    public static Specialist Create(string fullName)
    {
        // TODO: Добавить валидацию
        return new Specialist
        {
            FullName = fullName
        };
    }
    
    public void Update(string newFullName)
    {
        // TODO: Добавить валидацию
        FullName = newFullName;
    }
}