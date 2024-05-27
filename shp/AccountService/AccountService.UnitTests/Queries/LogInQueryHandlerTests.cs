using AccountService.Application.AccountUseCases.Handlers;
using AccountService.Application.AccountUseCases.Queries;
using AccountService.Core.Interfaces;
using AccountService.Core.Models;
using AccountService.UnitTests.Mocks;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace AccountService.UnitTests.Queries;

public class LogInQueryHandlerTests
{
    private readonly Mock<IAccountRepository> _mock;

    public LogInQueryHandlerTests()
    {
        _mock = MockRepositories.GetAccountRepository();
    }

    [Fact]
    public async Task Success_Account_Login()
    {
        var handler = new LogInQueryHandler(
            new PasswordHasher<Account>(), _mock.Object);

        var result = await handler.Handle(
            new LogInQuery("aboba123@gmail.com", "abobapswrd"), CancellationToken.None);

        Assert.NotNull(result);
        Assert.True(result.Email == "aboba123@gmail.com");
        Assert.True(result.Username == "aboba");
    }

    [Fact]
    public async Task WrongEmail_Account_Login()
    {
        var handler = new LogInQueryHandler(
            new PasswordHasher<Account>(), _mock.Object);

        var result = await handler.Handle(
            new LogInQuery("aboba13@gmail.com", "abobapswrd"), CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task WrongPassword_Account_Login()
    {
        var handler = new LogInQueryHandler(
            new PasswordHasher<Account>(), _mock.Object);

        var result = await handler.Handle(
            new LogInQuery("aboba123@gmail.com", "wrong"), CancellationToken.None);

        Assert.Null(result);
    }
}
