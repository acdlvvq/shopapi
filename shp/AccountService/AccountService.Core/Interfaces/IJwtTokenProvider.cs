using AccountService.Core.Models;

namespace AccountService.Core.Interfaces;

public interface IJwtTokenProvider
{
    string GetToken(Account account);
    string GetRefreshToken();
}
