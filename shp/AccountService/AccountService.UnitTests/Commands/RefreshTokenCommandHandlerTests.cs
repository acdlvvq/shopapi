using AccountService.Application.AccountUseCases.Commands;
using AccountService.Application.AccountUseCases.Handlers;
using AccountService.Core.Interfaces;
using AccountService.Infrastructure.Jwt;
using AccountService.UnitTests.Mocks;
using Moq;

namespace AccountService.UnitTests.Commands;

public class RefreshTokenCommandHandlerTests
{
    private readonly Mock<IAccountRepository> _mockAccountRepository;
    private readonly Mock<ITokenRepository> _mockTokenRepository;
    private readonly Mock<IJwtOptions> _mockJwtOptions;

    public RefreshTokenCommandHandlerTests()
    {
        _mockAccountRepository = MockRepositories.GetAccountRepository();
        _mockTokenRepository = MockRepositories.GetTokenRepository();
        _mockJwtOptions = MockOptions.GetJwtOptions();
    }

    [Fact]
    public async Task NoResponse_RefreshToken_NotFound()
    {
        var opt = _mockJwtOptions.Object;

        var handler = new RefreshTokenCommandHandler(
            _mockAccountRepository.Object,
            _mockTokenRepository.Object,
            new JwtTokenProvider(opt),
            opt);

        var result = await handler.Handle(
            new RefreshTokenCommand(Guid.NewGuid().ToString()), CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task NoResponse_RefreshToken_Expired()
    {
        var opt = _mockJwtOptions.Object;

        var handler = new RefreshTokenCommandHandler(
            _mockAccountRepository.Object,
            _mockTokenRepository.Object,
            new JwtTokenProvider(opt),
            opt);

        var result = await handler.Handle(
            new RefreshTokenCommand("expired_token"), CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task NoResponse_RefreshToken_AccountNotFound()
    {
        var opt = _mockJwtOptions.Object;

        var handler = new RefreshTokenCommandHandler(
            _mockAccountRepository.Object,
            _mockTokenRepository.Object,
            new JwtTokenProvider(opt),
            opt);

        var result = await handler.Handle(
            new RefreshTokenCommand("wrong_account_id"), CancellationToken.None);

        Assert.Null(result);
    }
}
