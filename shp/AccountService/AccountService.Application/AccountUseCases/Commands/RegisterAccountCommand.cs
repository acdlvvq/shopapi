using MediatR;

namespace AccountService.Application.AccountUseCases.Commands;

public record RegisterAccountCommand(
    string Email, string Username, string Password) : IRequest<int>;
