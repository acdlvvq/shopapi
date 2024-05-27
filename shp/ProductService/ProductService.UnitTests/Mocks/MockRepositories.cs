using Moq;
using ProductService.Core.Interfaces;
using ProductService.Core.Models;
using System.Linq.Expressions;

namespace ProductService.UnitTests.Mocks;

public static class MockRepositories
{
    public static Guid ID1 { get; set; } = Guid.Parse("80543fa3-1b3b-4b36-b3d1-7052eb3da3cf");
    public static Guid ID2 { get; set; } = Guid.Parse("56d27a50-dfd2-466a-8495-cb5631e776bc");

    public static Mock<IProductsRepository> GetProductsRepository()
    {
        List<Product> products = [
            Product.Create("coffee", "makes you phresh", Convert.ToDecimal(3.124), 10000, ID1),
            Product.Create("the siga", "makes you napas", Convert.ToDecimal(7.5), 20000, ID1),
            Product.Create("spotify", "makes you listen trapchik", Convert.ToDecimal(15.124124), 1, ID2)
        ];

        var repository = new Mock<IProductsRepository>();

        repository
            .Setup(r => r.DeleteAsync(It.IsAny<Guid>()))
            .Callback((Guid g) =>
            {
                var del = products.Find(p => p.Id == g);
                if (del is not null)
                    products.Remove(del);
            });

        repository
            .Setup(r => r.UpdateAsync(It.IsAny<Product>()))
            .Callback((Product p) => 
            {
                var upd = products.Find(pr => pr.Id == p.Id);
                upd?.SetName(p.Name)
                       .SetAvailableAmount(p.AvailableAmount)
                       .SetDescription(p.Description)
                       .SetPrice(p.Price);
            });

        repository
            .Setup(r => r.AddAsync(It.IsAny<Product>()))
            .ReturnsAsync((Product p) =>
            {
                products.Add(p);
                return p.Id;
            });

        repository
            .Setup(r => r.Get(It.IsAny<Expression<Func<Product, bool>>>()))
            .Returns((Expression<Func<Product, bool>> s) =>
            {
                return products
                    .Where(s.Compile())
                    .AsQueryable();
            });

        repository
            .Setup(r => r.GetAll())
            .Returns(() =>
            {
                return products
                    .AsQueryable();
            });

        return repository;
    }
}
