using FluentValidation;
using FluentValidation.AspNetCore;
using MemoryPlaces.Application.Mappings;
using MemoryPlaces.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace MemoryPlaces.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly)
        );
        services.AddAutoMapper(typeof(MemoryPlacesMappingProfile));
        services
            .AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly)
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
    }
}
