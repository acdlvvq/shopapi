using AccountService.Core.Models;
using MediatR;

namespace AccountService.Application.AccountUseCases.Queries;

public record LogInQuery(
    string Email, string Password) : IRequest<Account?>;
