namespace SmartFix.Domain.Aggregates;

public class Master : User
{
    public string PhoneNumber { get; private set; }
    public bool IsDeleted { get; private set; }

    private Master()
    {
    }

    public static Master Create(string email, string passwordHash, string name, string phoneNumber)
    {
        return new Master
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = passwordHash,
            Name = name,
            PhoneNumber = phoneNumber,
        };
    }

    public void Update(string email, string name, string phoneNumber)
    {
        Email = email;
        Name = name;
        PhoneNumber = phoneNumber;
    }

    public void Delete() => IsDeleted = true;
}