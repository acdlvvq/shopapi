using ProductService.Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ProductService.Infrastructure.Jwt;

public class JwtReader : IJwtReader
{
    public IEnumerable<KeyValuePair<string, string>> GetClaimsFromToken(JwtSecurityToken token)
    {
        return token.Claims
            .Select(c => KeyValuePair.Create(c.Type, c.Value));
    }

    public JwtSecurityToken GetTokenFromString(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        return jwt;
    }
}
