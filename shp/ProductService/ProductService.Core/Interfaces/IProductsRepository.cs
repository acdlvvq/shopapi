using ProductService.Core.Models;
using System.Linq.Expressions;

namespace ProductService.Core.Interfaces;

public interface IProductsRepository
{
    IQueryable<Product> Get(Expression<Func<Product, bool>> selector);
    IQueryable<Product> GetAll();

    Task<Guid> AddAsync(Product product);

    Task UpdateAsync(Product product);

    Task DeleteAsync(Guid id);

    Task<int> SaveChangesAsync();
}
