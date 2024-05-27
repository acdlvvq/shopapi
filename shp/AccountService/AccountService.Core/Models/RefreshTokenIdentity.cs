namespace AccountService.Core.Models;

public class RefreshTokenIdentity
{
    public Guid Id { get; private set; }
    public string Token { get; private set; } = string.Empty;
    public DateTime ExpiresIn { get; private set; }
    public bool IsExpired 
    { 
        get => DateTime.UtcNow > ExpiresIn;
    }

    public static RefreshTokenIdentity Create(
        Guid id, string token, DateTime expiresIn)
    {
        return new RefreshTokenIdentity
        {
            Id = id,
            Token = token,
            ExpiresIn = expiresIn
        };
    }

    public void SetToken(string token, DateTime expiresIn) 
    {
        Token = token;
        ExpiresIn = expiresIn;
    }
}
