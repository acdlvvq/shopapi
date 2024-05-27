using Microsoft.IdentityModel.Tokens;

namespace AccountService.Core.Interfaces;

public interface IJwtOptions
{
    string Audience { get; }
    int ExpiresIn { get; }
    string Issuer { get; }
    SymmetricSecurityKey Key { get; }
    DateTime NewRefreshTokenExpirationTime { get; }
}