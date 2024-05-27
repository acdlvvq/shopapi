using Microsoft.EntityFrameworkCore;
using ProductService.DA;
using ProductService.Core;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ProductService.Application;
using ProductService.Infrastructure;

namespace ProductService.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var key = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")!));
            var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER")!;
            var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")!;

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddProblemDetails();

            builder.Services
                .AddCore()
                .AddApplication()
                .AddInfrastructure()
                .AddDAL(
                    builder.Configuration.GetConnectionString("ProductsConnection")!);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = false;
                options.IncludeErrorDetails = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = key,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    IdentityData.AdminUserPolicyName,
                    p => p.RequireClaim(ClaimTypes.Role, "Admin"));
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
                context.Database.Migrate();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
