using TravelInspiration.API.Shared.Networking;

namespace TravelInspiration.API.Features.SearchDestinations;

public static class SearchDestinations
{
    public static void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/destinations", async (string? searchFor,
            ILoggerFactory logger,
            IDestinationSearchApiClient destinationSearchClient) =>
        {
            logger.CreateLogger("EndpointHandlers")
                .LogInformation("SearchDestionations feature called");

            var result = await destinationSearchClient.GetDestinationsAsync(searchFor, null);

            return Results.Ok(result);
        });
    }
}
