using AccountService.DA.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountService.DA;

public class AccountDbContext : DbContext
{
    public DbSet<AccountEntity> Accounts { get; set; }
    public DbSet<TokenEntity> Tokens { get; set; }

    public AccountDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountEntity>()
            .HasKey(a => a.Id);

        modelBuilder.Entity<TokenEntity>()
            .HasKey(t => t.Id);
    }
}
