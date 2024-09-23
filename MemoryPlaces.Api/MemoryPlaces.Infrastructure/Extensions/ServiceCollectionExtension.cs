using MemoryPlaces.Infrastructure.Persistance;
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
        services.AddDbContext<MemoryPlacesDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("MemoryPlacesDBConnectionString")
            )
        );
    }
}
