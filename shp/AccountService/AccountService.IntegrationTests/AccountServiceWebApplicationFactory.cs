using AccountService.Core.Interfaces;
using AccountService.DA;
using AccountService.Presentation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.Text;

namespace AccountService.IntegrationTests;

public class AccountServiceWebApplicationFactory
    : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<AccountDbContext>));

            services.AddSqlServer<AccountDbContext>(
                @"Server=(localdb)\mssqllocaldb;Database=accounts-test;Trusted_Connection=True;");
        
            var dbContext = GetDbContext(services);
            dbContext.Database.EnsureDeleted();
        });
    }

    private static AccountDbContext GetDbContext(IServiceCollection services)
    {
        var provider = services.BuildServiceProvider();
        var scope = provider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<AccountDbContext>();
    }

}
