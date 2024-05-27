using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Core.Interfaces;
using ProductService.DA.Repositories;

namespace ProductService.DA;

public static class DependencyInjection
{
    public static IServiceCollection AddDAL(
        this IServiceCollection services, string connection)
    {
        return services
            .AddScoped<IProductsRepository, ProductsRepository>()
            .AddDbContext<ProductDbContext>(
                options => { options.UseSqlServer(connection); });
    }
}
