using AccountService.Core.Interfaces;
using AccountService.Infrastructure.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AccountService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        return services
            .AddSingleton<IJwtOptions, JwtOptions>()
            .AddSingleton<IJwtTokenProvider, JwtTokenProvider>();
    }
}
