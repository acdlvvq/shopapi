namespace AccountService.Core.Models;

public class TokenResponse
{
    public string AccessToken { get; private set; } = string.Empty;
    public string RefreshToken { get; private set; } = string.Empty;

    public static TokenResponse Create(
        string accessToken, string refreshToken)
    {
        return new TokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}
