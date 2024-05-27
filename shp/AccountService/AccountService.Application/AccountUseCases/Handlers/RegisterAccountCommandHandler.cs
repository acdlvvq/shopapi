using AccountService.Application.AccountUseCases.Commands;
using AccountService.Core.Enums;
using AccountService.Core.Interfaces;
using AccountService.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AccountService.Application.AccountUseCases.Handlers;

public class RegisterAccountCommandHandler : IRequestHandler<RegisterAccountCommand, int>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IPasswordHasher<Account> _passwordHasher;

    public RegisterAccountCommandHandler(
        IAccountRepository repository, IPasswordHasher<Account> passwordHasher)
    {
        _accountRepository = repository;
        _passwordHasher = passwordHasher;
    }

    public async Task<int> Handle(RegisterAccountCommand request, CancellationToken cancellationToken)
    {
        var account = Account.Create(
            request.Email, request.Username, AccountRole.DefaultUser);

        var hashed = _passwordHasher.HashPassword(account, request.Password);
        account.ChangePassword(hashed);

        var cantCreate = _accountRepository
            .Get(a => a.Email == request.Email || a.Username == request.Username)
            .Any();

        if (cantCreate)
        {
            return 0;
        }

        await _accountRepository.AddAsync(account);
        return await _accountRepository.SaveChangesAsync();
    }
}
