using AccountService.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AccountService.Infrastructure.Jwt;

public class JwtOptions : IJwtOptions
{
    private readonly int _refreshTokenExpiresIn;

    public SymmetricSecurityKey Key { get; private set; }
    public int ExpiresIn { get; private set; }
    public string Issuer { get; private set; }
    public string Audience { get; private set; }
    public DateTime NewRefreshTokenExpirationTime
    {
        get => DateTime.UtcNow.AddMinutes(_refreshTokenExpiresIn);
    }

    public JwtOptions(IConfiguration configuration)
    {
        var key = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(configuration["JWT_KEY"]!));
        var expiresIn = Convert.ToInt32(
            configuration["JWT_EXPIRES"]!);
        var refreshTokenExpiresIn = Convert.ToInt32(
            configuration["JWT_REFRESH_EXPIRES"]!);
        var issuer = configuration["JWT_ISSUER"]!;
        var audience = configuration["JWT_AUDIENCE"]!;

        _refreshTokenExpiresIn = refreshTokenExpiresIn;
        Key = key;
        ExpiresIn = expiresIn;
        Issuer = issuer;
        Audience = audience;
    }
}
