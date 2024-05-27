using AccountService.Application.AccountUseCases.Commands;
using AccountService.Application.AccountUseCases.Handlers;
using AccountService.Core.Interfaces;
using AccountService.Core.Models;
using AccountService.Infrastructure.Jwt;
using AccountService.UnitTests.Mocks;
using Moq;

namespace AccountService.UnitTests.Commands;

public class GetTokensCommandHandlerTests
{
    private readonly Mock<ITokenRepository> _mockTokenRepository;
    private readonly Mock<IJwtOptions> _mockJwtOptions;

    public GetTokensCommandHandlerTests()
    {
        _mockTokenRepository = MockRepositories.GetTokenRepository();
        _mockJwtOptions = MockOptions.GetJwtOptions();
    }

    [Fact]
    public async Task FirstLogin_RefreshTokenIdentity_Created()
    {
        var handler = new GetTokensCommandHandler(
            _mockTokenRepository.Object,
            new JwtTokenProvider(_mockJwtOptions.Object),
            _mockJwtOptions.Object);
        var account = Account.Create("abc@gmail.com", "abc", Core.Enums.AccountRole.DefaultUser);

        var result = await handler.Handle(
            new GetTokensCommand(account), CancellationToken.None);

        Assert.NotNull(result);
        Assert.NotNull(_mockTokenRepository.Object.GetByAccountId(account.Id));
    }

    [Fact]
    public async Task NotFirstLogin_RefreshTokenIdentity_Updated()
    {
        var handler = new GetTokensCommandHandler(
            _mockTokenRepository.Object,
            new JwtTokenProvider(_mockJwtOptions.Object),
            _mockJwtOptions.Object);
        var account = Account.Create("abc@gmail.com", "abc", Core.Enums.AccountRole.DefaultUser);

        var first = await handler.Handle(
            new GetTokensCommand(account), CancellationToken.None);
        var second = await handler.Handle(
            new GetTokensCommand(account), CancellationToken.None);

        var firstToken = first.RefreshToken;
        var secondToken = second.RefreshToken;

        Assert.NotNull(first);
        Assert.NotNull(second);
        Assert.NotNull(firstToken);
        Assert.NotNull(secondToken);
        Assert.True(firstToken != secondToken);
    }
}
