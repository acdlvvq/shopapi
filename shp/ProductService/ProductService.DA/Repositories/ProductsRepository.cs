using Microsoft.EntityFrameworkCore;
using ProductService.Core.Interfaces;
using ProductService.Core.Models;
using ProductService.DA.Entities;
using System.Linq.Expressions;

namespace ProductService.DA.Repositories;

public class ProductsRepository : IProductsRepository
{
    private readonly ProductDbContext _context;

    public ProductsRepository(ProductDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> AddAsync(Product product)
    {
        var entity = new ProductEntity
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            AvailableAmount = product.AvailableAmount,
            CreatorId = product.CreatorId,
            CreatedAt = product.CreatedAt
        };

        await _context.Products.AddAsync(entity);
        return entity.Id;
    }

    public async Task DeleteAsync(Guid id)
    {
        await _context.Products
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync();
    }

    public IQueryable<Product> Get(Expression<Func<Product, bool>> selector)
    {
        return _context.Products
            .AsNoTracking()
            .Select(p => new Product(
                p.Id, p.Name, p.Description, p.Price, p.AvailableAmount, p.CreatorId, p.CreatedAt))
            .Where(selector.Compile())
            .AsQueryable();
    }

    public IQueryable<Product> GetAll()
    {
        return _context.Products
            .AsNoTracking()
            .Select(p => new Product(
                p.Id, p.Name, p.Description, p.Price, p.AvailableAmount, p.CreatorId, p.CreatedAt))
            .AsQueryable();
    }

    public async Task<int> SaveChangesAsync()
    {
        var saved = await _context.SaveChangesAsync();

        return saved;
    }

    public Task UpdateAsync(Product product)
    {
        var entity = new ProductEntity
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            AvailableAmount = product.AvailableAmount,
            CreatorId = product.CreatorId,
            CreatedAt = product.CreatedAt
        };

        return Task.Run(
            () => _context.Products.Update(entity));
    }
}
