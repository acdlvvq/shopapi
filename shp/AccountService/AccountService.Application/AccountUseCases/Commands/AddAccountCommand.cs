using AccountService.Core.Enums;
using MediatR;

namespace AccountService.Application.AccountUseCases.Commands;

public record AddAccountCommand(
    string Email, string Username, string Password, AccountRole Role) : IRequest<Guid?>;
