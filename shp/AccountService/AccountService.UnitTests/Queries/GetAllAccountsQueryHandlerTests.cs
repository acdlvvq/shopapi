using AccountService.Application.AccountUseCases.Handlers;
using AccountService.Application.AccountUseCases.Queries;
using AccountService.Core.Interfaces;
using AccountService.Core.Models;
using AccountService.UnitTests.Mocks;
using Moq;

namespace AccountService.UnitTests.Queries;

public class GetAllAccountsQueryHandlerTests
{
    private readonly Mock<IAccountRepository> _mock;

    public GetAllAccountsQueryHandlerTests()
    {
        _mock = MockRepositories.GetAccountRepository();
    }

    [Fact]
    public async Task Get_WhenCalled_ReturnsAllItems()
    {
        var handler = new GetAllAccountsQueryHandler(_mock.Object);

        var result = await handler.Handle(new GetAllAccountsQuery(), CancellationToken.None);

        Assert.IsAssignableFrom<IEnumerable<Account>>(result);
        Assert.True(result.Count() == 3);
    }
}