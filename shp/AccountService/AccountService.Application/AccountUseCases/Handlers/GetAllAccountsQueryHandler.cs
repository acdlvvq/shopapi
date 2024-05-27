using AccountService.Application.AccountUseCases.Queries;
using AccountService.Core.Interfaces;
using AccountService.Core.Models;
using MediatR;

namespace AccountService.Application.AccountUseCases.Handlers;

public class GetAllAccountsQueryHandler 
    : IRequestHandler<GetAllAccountsQuery, IEnumerable<Account>>
{
    private readonly IAccountRepository _repository;

    public GetAllAccountsQueryHandler(IAccountRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Account>> Handle(
        GetAllAccountsQuery request, CancellationToken cancellationToken)
    {
        return Task.Run(
            () => _repository.GetAll().AsEnumerable());
    }
}
