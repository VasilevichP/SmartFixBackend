using Microsoft.AspNetCore.Identity;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Domain.Aggregates;

public abstract class User
{
    public Guid Id { get; protected set; }
    public string Email { get; protected set; }
    public string PasswordHash { get; protected set; }
    public string? Name { get; protected set; }

    protected User()
    {
    }
}