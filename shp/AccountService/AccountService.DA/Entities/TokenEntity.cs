namespace AccountService.DA.Entities;

public class TokenEntity
{
    public Guid Id { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExperesIn { get; set; }
}
