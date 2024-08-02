using TravelInspiration.API.Shared.Networking;
using TravelInspiration.API.Shared.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace TravelInspiration.API;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IDestinationSearchApiClient, DestinationSearchApiClient>();

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

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
