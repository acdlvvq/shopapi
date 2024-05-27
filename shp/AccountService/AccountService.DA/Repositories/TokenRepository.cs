using AccountService.Core.Interfaces;
using AccountService.Core.Models;
using AccountService.DA.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AccountService.DA.Repositories;

public class TokenRepository : ITokenRepository
{
    private readonly AccountDbContext _context;

    public TokenRepository(AccountDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> AddAsync(RefreshTokenIdentity identity)
    {
        var entity = new TokenEntity
        {
            Id = identity.Id,
            RefreshToken = identity.Token,
            ExperesIn = identity.ExpiresIn
        };

        await _context.AddAsync(entity);
        return entity.Id;
    }

    public IQueryable<RefreshTokenIdentity> Get(Expression<Func<RefreshTokenIdentity, bool>> selector)
    {
        return _context
            .Tokens.AsNoTracking()
            .Select(t => RefreshTokenIdentity.Create(
                t.Id, t.RefreshToken, t.ExperesIn))
            .Where(selector.Compile())
            .AsQueryable();
    }

    public async Task<RefreshTokenIdentity?> GetByAccountId(Guid id)
    {
        return await _context
            .Tokens.AsNoTracking()
            .Where(t => t.Id == id)
            .Select(t => RefreshTokenIdentity.Create(t.Id, t.RefreshToken, t.ExperesIn))
            .FirstOrDefaultAsync();
    }

    public async Task<int> SaveChangesAsync()
    {
        var saved = await _context.SaveChangesAsync();

        return saved;
    }

    public Task UpdateAsync(RefreshTokenIdentity identity)
    {
        var entity = new TokenEntity
        {
            Id = identity.Id,
            RefreshToken = identity.Token,
            ExperesIn = identity.ExpiresIn
        };

        return Task.Run(
            () => _context.Update(entity));
    }
}
