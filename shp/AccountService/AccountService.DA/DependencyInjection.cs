using AccountService.Core.Interfaces;
using AccountService.DA.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AccountService.DA;

public static class DependencyInjection
{
    public static IServiceCollection AddDAL(
        this IServiceCollection services, string connection) 
    { 
        return services
            .AddScoped<IAccountRepository, AccountRepository>()
            .AddScoped<ITokenRepository, TokenRepository>()
            .AddDbContext<AccountDbContext>(
                options => { options.UseSqlServer(connection); });
    }
}
