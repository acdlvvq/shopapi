using AccountService.Core.Interfaces;
using AccountService.Core.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AccountService.Infrastructure.Jwt;

public class JwtTokenProvider : IJwtTokenProvider
{
    private readonly IJwtOptions _options;

    public JwtTokenProvider(IJwtOptions options)
    {
        _options = options;
    }

    public string GetRefreshToken()
    {
        return Guid.NewGuid().ToString();  
    }

    public string GetToken(Account account)
    {
        var expires = DateTime.UtcNow;

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, account.Username),
                new Claim(JwtRegisteredClaimNames.Email, account.Email),
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Role, account.Role.ToString())
            ]),
            Expires = expires.AddMinutes(_options.ExpiresIn),
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            SigningCredentials = new(_options.Key, SecurityAlgorithms.HmacSha256),
        };

        var handler = new JwtSecurityTokenHandler();
        return handler.CreateEncodedJwt(descriptor);
    }
}
