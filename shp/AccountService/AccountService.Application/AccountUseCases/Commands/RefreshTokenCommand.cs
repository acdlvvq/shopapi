using AccountService.Core.Models;
using MediatR;

namespace AccountService.Application.AccountUseCases.Commands;

public record RefreshTokenCommand(
    string RefreshToken) : IRequest<TokenResponse?>;
