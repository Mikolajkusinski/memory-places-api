using System.Text.Json.Serialization;
using MemoryPlaces.Domain.RepositoryInterfaces;
using MemoryPlaces.Infrastructure.Persistance;
using MemoryPlaces.Infrastructure.Repositories;
using MemoryPlaces.Infrastructure.Seeders;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MemoryPlaces.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });
        services.AddDbContext<MemoryPlacesDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("MemoryPlacesDBConnectionString")
            )
        );

        services.AddScoped<DatabaseSeeder>();
        services.AddScoped<IAccountRepository, AccountRepository>();
    }
}
