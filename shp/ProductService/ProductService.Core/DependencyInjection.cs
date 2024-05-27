using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Core.Models;
using ProductService.Core.Validators;

namespace ProductService.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(
        this IServiceCollection services)
    {
        return services.AddScoped<IValidator<Product>, ProductValidator>();
    }
}
