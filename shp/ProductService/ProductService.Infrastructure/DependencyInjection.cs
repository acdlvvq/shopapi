using Microsoft.Extensions.DependencyInjection;
using ProductService.Core.Interfaces;
using ProductService.Infrastructure.Jwt;

namespace ProductService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        return services
            .AddSingleton<IJwtReader, JwtReader>();
    }
}
