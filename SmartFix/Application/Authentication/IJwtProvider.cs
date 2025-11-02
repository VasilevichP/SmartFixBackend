using SmartFix.Domain.Aggregates;

namespace SmartFix.Application.Authentication;

public interface IJwtProvider
{
    string GenerateToken(User user);
}