using AccountService.Application.AccountUseCases.Commands;
using AccountService.Core.Interfaces;
using AccountService.Core.Models;
using MediatR;

namespace AccountService.Application.AccountUseCases.Handlers;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenResponse?>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITokenRepository _tokenRepository;
    private readonly IJwtTokenProvider _tokenProvider;
    private readonly IJwtOptions _jwtOptions;

    public RefreshTokenCommandHandler(
        IAccountRepository accountRepository,
        ITokenRepository tokenRepository,
        IJwtTokenProvider tokenProvider,
        IJwtOptions jwtOptions)
    {
        _accountRepository = accountRepository;
        _tokenRepository = tokenRepository;
        _tokenProvider = tokenProvider;
        _jwtOptions = jwtOptions;
    }

    public async Task<TokenResponse?> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var token = _tokenRepository
            .Get(t => t.Token == request.RefreshToken)
            .FirstOrDefault();

        if (token is null)
            return null;
        if (token.IsExpired)
            return null;

        var account = _accountRepository
            .Get(a => a.Id == token.Id)
            .FirstOrDefault();

        if (account is null)
            return null;

        var accessToken = _tokenProvider.GetToken(account);
        var refreshToken = _tokenProvider.GetRefreshToken();

        token.SetToken(refreshToken, _jwtOptions.NewRefreshTokenExpirationTime);
        await _tokenRepository.UpdateAsync(token);
        await _tokenRepository.SaveChangesAsync();

        return TokenResponse.Create(accessToken, refreshToken);
    }
}
