using Microsoft.AspNetCore.Identity;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Domain.Aggregates;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public Role Role { get; private set; }

    public string? Name { get; private set; }
    public string? PhoneNumber { get; private set; }
    private User() { } 

    public static User CreateClient(string email, string passwordHash, string? name, string? phoneNumber)
    {
        return new User
        {
            Email = email,
            PasswordHash = passwordHash,
            Role = Role.Client,
            PhoneNumber = phoneNumber,
            Name = name
        };
    }
 public static User CreateManager(string email, string passwordHash)
    {
        return new User
        {
            Email = email,
            PasswordHash = passwordHash,
            Role = Role.Manager
        };
    }
}