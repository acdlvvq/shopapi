using Microsoft.Extensions.DependencyInjection;

namespace AccountService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        return services.AddMediatR(config => 
            config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
    }
}
