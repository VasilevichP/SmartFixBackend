using Microsoft.AspNetCore.Identity;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Domain.Aggregates;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public Role Role { get; private set; }

    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? MiddleName { get; private set; }
    public string? PhoneNumber { get; private set; }
    private User() { } 

    public static User CreateClient(string email, string passwordHash, string firstName, string lastName, string phoneNumber, string? middleName = null)
    {
        return new User
        {
            Email = email,
            PasswordHash = passwordHash,
            Role = Role.Client,
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = phoneNumber,
            MiddleName = middleName
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