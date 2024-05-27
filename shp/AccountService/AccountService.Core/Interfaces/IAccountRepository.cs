using AccountService.Core.Models;
using System.Linq.Expressions;

namespace AccountService.Core.Interfaces;

public interface IAccountRepository
{
    IQueryable<Account> Get(Expression<Func<Account, bool>> selector);
    IQueryable<Account> GetAll();

    Task<Guid> AddAsync(Account account);

    Task UpdateAsync(Account account);

    Task DeleteAsync(Guid id);

    Task<int> SaveChangesAsync();
}
