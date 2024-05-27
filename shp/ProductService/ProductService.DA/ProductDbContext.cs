using Microsoft.EntityFrameworkCore;
using ProductService.DA.Entities;

namespace ProductService.DA;

public class ProductDbContext : DbContext
{
    public DbSet<ProductEntity> Products { get; set; }

    public ProductDbContext(DbContextOptions options) 
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductEntity>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<ProductEntity>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);
    }
}
