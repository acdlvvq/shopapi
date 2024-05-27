using AccountService.Application.AccountUseCases.Commands;
using AccountService.Application.AccountUseCases.Handlers;
using AccountService.Core.Interfaces;
using AccountService.Core.Models;
using AccountService.UnitTests.Mocks;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace AccountService.UnitTests.Commands;

public class AddAccountCommandHandlerTests
{
    private readonly Mock<IAccountRepository> _mock;

    public AddAccountCommandHandlerTests()
    {
        _mock = MockRepositories.GetAccountRepository();
    }

    [Fact]
    public async Task Valid_Account_Added()
    {
        var handler = new AddAccountCommandHandler(_mock.Object, new PasswordHasher<Account>());

        var result = await handler.Handle(
            new AddAccountCommand("new@gmail.com", "new", "newpswrd", Core.Enums.AccountRole.DefaultUser),
            CancellationToken.None);

        Assert.NotNull(result);
        Assert.True(_mock.Object.GetAll().Count() == 4);
    }

    [Fact]
    public async Task Invalid_Account_Added()
    {
        var handler = new AddAccountCommandHandler(_mock.Object, new PasswordHasher<Account>());

        var result = await handler.Handle(
            new AddAccountCommand("aboba123@gmail.com", "new", "newpswrd", Core.Enums.AccountRole.DefaultUser),
            CancellationToken.None);

        Assert.Null(result);
        Assert.True(_mock.Object.GetAll().Count() == 3);
    }
}
