using AccountService.Core.Enums;

namespace AccountService.Core.Models;

public class Account
{
    public Account(
        Guid id,
        string email,
        string username,
        string password,
        AccountRole role, 
        DateTime createdAt)
    {
        Id = id;
        Email = email;
        Username = username;
        Password = password;
        Role = role;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string Username { get; private set; } = string.Empty;
    public string Password { get; private set; } = string.Empty;
    public AccountRole Role { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static Account Create(
        string email, string username, AccountRole role)
    {
        return new Account(
            Guid.NewGuid(),
            email,
            username,
            string.Empty,
            role,
            DateTime.UtcNow);
    }

    public void ChangePassword(string password)
        => Password = password;
}
