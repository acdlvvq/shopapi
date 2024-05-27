using AccountService.Core.Models;
using MediatR;

namespace AccountService.Application.AccountUseCases.Commands;

public record GetTokensCommand(
    Account Account) : IRequest<TokenResponse>;
