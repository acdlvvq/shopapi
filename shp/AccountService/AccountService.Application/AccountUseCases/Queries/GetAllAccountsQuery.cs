using AccountService.Core.Models;
using MediatR;

namespace AccountService.Application.AccountUseCases.Queries;

public record GetAllAccountsQuery() 
    : IRequest<IEnumerable<Account>>;
