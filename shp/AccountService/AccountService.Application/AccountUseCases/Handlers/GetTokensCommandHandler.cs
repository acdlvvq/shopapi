using AccountService.Application.AccountUseCases.Commands;
using AccountService.Core.Interfaces;
using AccountService.Core.Models;
using MediatR;

namespace AccountService.Application.AccountUseCases.Handlers;

public class GetTokensCommandHandler : IRequestHandler<GetTokensCommand, TokenResponse>
{
    private readonly ITokenRepository _tokenRepository;
    private readonly IJwtTokenProvider _tokenProvider;
    private readonly IJwtOptions _jwtOptions;

    public GetTokensCommandHandler(
        ITokenRepository tokenRepository,
        IJwtTokenProvider tokenProvider,
        IJwtOptions jwtOptions)
    {
        _tokenRepository = tokenRepository;
        _tokenProvider = tokenProvider;
        _jwtOptions = jwtOptions;
    }

    public async Task<TokenResponse> Handle(GetTokensCommand request, CancellationToken cancellationToken)
    {
        var accessToken = _tokenProvider.GetToken(request.Account);
        var refreshToken = _tokenProvider.GetRefreshToken();
        var accountIdentity = await _tokenRepository.GetByAccountId(request.Account.Id);

        if (accountIdentity is not null)
        {
            accountIdentity.SetToken(refreshToken, _jwtOptions.NewRefreshTokenExpirationTime);
            await _tokenRepository.UpdateAsync(accountIdentity);
        }
        else
        {
            var identity = RefreshTokenIdentity.Create(
                request.Account.Id, refreshToken, _jwtOptions.NewRefreshTokenExpirationTime);
            await _tokenRepository.AddAsync(identity);
        }

        await _tokenRepository.SaveChangesAsync();
        return TokenResponse.Create(accessToken, refreshToken);
    }
}
