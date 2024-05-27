using AccountService.DA;
using AccountService.Application;
using AccountService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.AspNetCore.Identity;
using AccountService.Core.Models;
using Microsoft.IdentityModel.Tokens;
using AccountService.Core.Enums;
using System.Security.Claims;

namespace AccountService.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var key = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(builder.Configuration["JWT_KEY"]!));
            var issuer = builder.Configuration["JWT_ISSUER"]!;
            var audience = builder.Configuration["JWT_AUDIENCE"]!;

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();

            builder.Services.AddDAL(
                builder.Configuration.GetConnectionString("AccountsConnection")!);
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure();

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
                    p => p.RequireClaim(ClaimTypes.Role, AccountRole.Admin.ToString()));
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AccountDbContext>();
                db.Database.Migrate();
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
