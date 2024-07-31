using TravelInspiration.API.Shared.Networking;
using TravelInspiration.API.Shared.Persistence;
using Microsoft.EntityFrameworkCore;

namespace TravelInspiration.API;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IDestinationSearchApiClient, DestinationSearchApiClient>();
        return services;
    }

    public static IServiceCollection RegisterPersistenceServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<TravelInspirationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("TravelInspirationDbConnection")));

        return services;
    }
}
