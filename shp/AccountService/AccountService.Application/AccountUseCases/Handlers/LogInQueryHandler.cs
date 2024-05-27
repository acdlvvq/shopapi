using AccountService.Application.AccountUseCases.Queries;
using AccountService.Core.Interfaces;
using AccountService.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Application.AccountUseCases.Handlers;

public class LogInQueryHandler : IRequestHandler<LogInQuery, Account?>
{
    private readonly IPasswordHasher<Account> _passwordHasher;
    private readonly IAccountRepository _accountRepository;

    public LogInQueryHandler(
        IPasswordHasher<Account> passwordHasher, IAccountRepository accountRepository)
    {
        _passwordHasher = passwordHasher;
        _accountRepository = accountRepository;
    }

    public async Task<Account?> Handle(LogInQuery request, CancellationToken cancellationToken)
    {
        var account = await Task.Run(() =>
             _accountRepository
                .Get(a => request.Email == a.Email)
                .FirstOrDefault());

        if (account is null)
        {
            return null;
        }
        
        return _passwordHasher.VerifyHashedPassword(
            account, account.Password, request.Password) is PasswordVerificationResult.Failed ? null : account;
    }
}
