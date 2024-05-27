using AccountService.Application.AccountUseCases.Commands;
using AccountService.Core.Interfaces;
using AccountService.Core.Models;
using AccountService.DA.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AccountService.Application.AccountUseCases.Handlers;

public class AddAccountCommandHandler
    : IRequestHandler<AddAccountCommand, Guid?>
{
    private readonly IAccountRepository _repository;
    private readonly IPasswordHasher<Account> _passwordHasher;

    public AddAccountCommandHandler(
        IAccountRepository repository, IPasswordHasher<Account> passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid?> Handle(AddAccountCommand request, CancellationToken cancellationToken)
    {
        var account = Account.Create(
            request.Email, request.Username, request.Role);

        var hashed = _passwordHasher.HashPassword(account, request.Password);
        account.ChangePassword(hashed);

        var cantCreate = _repository
            .Get(a => a.Email == request.Email || a.Username == request.Username)
            .Any();

        if (cantCreate)
        {
            return null;
        }

        var id = await _repository.AddAsync(account);
        await _repository.SaveChangesAsync();

        return id;
    }
}
