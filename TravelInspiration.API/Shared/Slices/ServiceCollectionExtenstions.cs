using System.Reflection;

namespace TravelInspiration.API.Shared.Slices;

public static class ServiceCollectionExtenstions
{
    public static IServiceCollection RegisterSlices(this IServiceCollection services)
    {
        var currentAssembly = Assembly.GetExecutingAssembly();

        var slices = currentAssembly.GetTypes().Where(x => 
            typeof(ISlice).IsAssignableFrom(x) && 
            x != typeof(ISlice) &&
            x.IsPublic &&
            !x.IsAbstract);

        foreach (var slice in slices)
        {
            services.AddSingleton(typeof(ISlice), slice);
        }

        return services;
    }
}
