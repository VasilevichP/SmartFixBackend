namespace SmartFix.Domain.Aggregates;

public class Manager:User
{
    private Manager() { }

    public static Manager Create(string email, string passwordHash, string name)
    {
        return new Manager
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = passwordHash,
            Name = name,
        };
    }
}