using AccountService.Core.Enums;
using AccountService.Core.Interfaces;
using AccountService.Core.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Linq.Expressions;

namespace AccountService.UnitTests.Mocks;

public static class MockRepositories
{
    public static Mock<IAccountRepository> GetAccountRepository()
    {
        List<Account> accounts = [];
        var hasher = new PasswordHasher<Account>();

        var aboba = Account.Create("aboba123@gmail.com", "aboba", AccountRole.DefaultUser);
        var abobaHashed = hasher.HashPassword(aboba, "abobapswrd");
        aboba.ChangePassword(abobaHashed);

        var sandali = Account.Create("sandali@gmail.com", "sandali", AccountRole.DefaultUser);
        var sandaliHashed = hasher.HashPassword(sandali, "kumarchik");
        sandali.ChangePassword(sandaliHashed);

        var admin = Account.Create("admin@gmail.com", "admin", AccountRole.Admin);
        var adminHashed = hasher.HashPassword(admin, "admin");
        admin.ChangePassword(adminHashed);

        accounts.Add(aboba);
        accounts.Add(sandali);
        accounts.Add(admin);

        var repository = new Mock<IAccountRepository>();

        repository
            .Setup(r => r.GetAll())
            .Returns(accounts.AsQueryable());

        repository
            .Setup(r => r.Get(It.IsAny<Expression<Func<Account, bool>>>()))
            .Returns((Expression<Func<Account, bool>> s) => accounts.Where(s.Compile()).AsQueryable());

        repository
            .Setup(r => r.AddAsync(It.IsAny<Account>()))
            .ReturnsAsync((Account a) => 
            {
                accounts.Add(a);
                return a.Id;
            });

        return repository;
    }

    public static Mock<ITokenRepository> GetTokenRepository()
    {
        List<RefreshTokenIdentity> tokens = [
            RefreshTokenIdentity.Create(Guid.NewGuid(), Guid.NewGuid().ToString(), DateTime.UtcNow.AddHours(1)),
            RefreshTokenIdentity.Create(Guid.NewGuid(), "wrong_account_id", DateTime.UtcNow.AddHours(2)),
            RefreshTokenIdentity.Create(Guid.NewGuid(), "expired_token", DateTime.UtcNow.AddSeconds(1)),
        ];

        var repository = new Mock<ITokenRepository>();

        repository
            .Setup(r => r.GetByAccountId(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) =>
            {
                return tokens
                    .Where(t => t.Id == id)
                    .FirstOrDefault();
            });

        repository
            .Setup(r => r.UpdateAsync(It.IsAny<RefreshTokenIdentity>()))
            .Callback((RefreshTokenIdentity t) =>
            {
                var token = tokens
                    .Where(tk => tk.Id == t.Id)
                    .FirstOrDefault();

                token?.SetToken(t.Token, t.ExpiresIn);
            });

        repository
            .Setup(r => r.AddAsync(It.IsAny<RefreshTokenIdentity>()))
            .ReturnsAsync((RefreshTokenIdentity t) =>
            {
                tokens.Add(t);
                return t.Id;
            });

        repository
            .Setup(r => r.Get(It.IsAny<Expression<Func<RefreshTokenIdentity, bool>>>()))
            .Returns((Expression<Func<RefreshTokenIdentity, bool>> s) =>
            {
                return tokens
                    .Where(s.Compile())
                    .AsQueryable();
            });

        return repository;
    }
}
