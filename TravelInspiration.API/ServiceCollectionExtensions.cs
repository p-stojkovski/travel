using TravelInspiration.API.Shared.Networking;
using TravelInspiration.API.Shared.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TravelInspiration.API.Shared.Slices;
using TravelInspiration.API.Shared.Behaviours;
using TravelInspiration.API.Shared.Metrics;
using FluentValidation;
using TravelInspiration.API.Shared.Security;

namespace TravelInspiration.API;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IDestinationSearchApiClient, DestinationSearchApiClient>();
        services.RegisterSlices();

        var currentAssembly = Assembly.GetExecutingAssembly();
        services.AddAutoMapper(currentAssembly);
        services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssembly(currentAssembly)
                .AddOpenRequestPreProcessor(typeof(LoggingBehaviour<>))
                .AddOpenBehavior(typeof(ModelValidationBehaviour<,>))
                .AddOpenBehavior(typeof(HandlerPerformanceMetricBehaviour<,>));
        });

        services.AddValidatorsFromAssembly(currentAssembly);
        services.AddSingleton<HandlerPerformanceMetric>();

        services.AddScoped<ICurrentUserService, CurrentUserService>();

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
