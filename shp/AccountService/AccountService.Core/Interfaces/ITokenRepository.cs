using AccountService.Core.Models;
using System.Linq.Expressions;

namespace AccountService.Core.Interfaces;

public interface ITokenRepository
{
    IQueryable<RefreshTokenIdentity> Get(Expression<Func<RefreshTokenIdentity, bool>> selector);
    Task<RefreshTokenIdentity?> GetByAccountId(Guid id);

    Task<Guid> AddAsync(RefreshTokenIdentity identity);

    Task UpdateAsync(RefreshTokenIdentity identity);

    Task<int> SaveChangesAsync();
}
