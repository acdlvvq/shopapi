using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ProductService.Core.Interfaces;

public interface IJwtReader
{
    JwtSecurityToken GetTokenFromString(string token);
    IEnumerable<KeyValuePair<string, string>> GetClaimsFromToken(JwtSecurityToken token);
}
