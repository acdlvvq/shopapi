using AccountService.Core.Enums;

namespace AccountService.DA.Entities;

public class AccountEntity
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public AccountRole Role { get; set; }
    public DateTime CreatedAt { get; set; }
}
