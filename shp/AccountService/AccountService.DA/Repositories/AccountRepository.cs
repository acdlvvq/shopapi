using AccountService.Core.Interfaces;
using AccountService.Core.Models;
using AccountService.DA.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AccountService.DA.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AccountDbContext _context;

    public AccountRepository(AccountDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> AddAsync(Account account)
    {
        var entity = new AccountEntity()
        {
            Id = account.Id,
            Email = account.Email,
            Username = account.Username,
            Password = account.Password,
            Role = account.Role,
            CreatedAt = account.CreatedAt
        };

        await _context.Accounts.AddAsync(entity);
        return entity.Id;
    }

    public async Task DeleteAsync(Guid id)
    {
        await _context.Accounts.Where(a => a.Id == id).ExecuteDeleteAsync();
    }

    public IQueryable<Account> Get(Expression<Func<Account, bool>> selector)
    {
        return _context
            .Accounts.AsNoTracking()
            .Select(a => 
                new Account(a.Id, a.Email, a.Username, a.Password, a.Role, a.CreatedAt))
            .Where(selector.Compile())
            .AsQueryable();
    }

    public IQueryable<Account> GetAll()
    {
        return _context
            .Accounts.AsNoTracking()
            .Select(a =>
                new Account(a.Id, a.Email, a.Username, a.Password, a.Role, a.CreatedAt))
            .AsQueryable();
    }

    public async Task<int> SaveChangesAsync()
    {
        var saved = await _context.SaveChangesAsync();

        return saved;
    }

    public async Task UpdateAsync(Account account)
    {
        var entity = new AccountEntity()
        {
            Id = account.Id,
            Email = account.Email,
            Username = account.Username,
            Password = account.Password,
            Role = account.Role,
            CreatedAt = account.CreatedAt
        };

        await Task.Run(
            () => _context.Accounts.Update(entity));
    }
}
