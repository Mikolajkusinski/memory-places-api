using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using MemoryPlaces.Application.ApplicationUser;
using MemoryPlaces.Application.Interfaces;
using MemoryPlaces.Application.Mappings;
using MemoryPlaces.Application.Settings;
using MemoryPlaces.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace MemoryPlaces.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var authenticationSettings = new AuthenticationSettings();
        configuration.GetSection("Authentication").Bind(authenticationSettings);

        services.AddSingleton(authenticationSettings);

        services
            .AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = "Bearer";
                option.DefaultScheme = "Bearer";
                option.DefaultChallengeScheme = "Bearer";
            })
            .AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authenticationSettings.JwtIssuer,
                    ValidAudience = authenticationSettings.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)
                    ),
                };
            });

        services.AddAuthorization();

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly)
        );

        services.AddAutoMapper(typeof(MemoryPlacesMappingProfile));

        services
            .AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly)
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters();

        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IUserContext, UserContext>();
    }
}
