using SmartFix.Application.Authentication;

namespace SmartFix.Infrastructure.Authentication;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    public bool VerifyPassword(string passwordHash, string providedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(providedPassword, passwordHash);
    }
}