using TravelInspiration.API.Shared.Networking;

namespace TravelInspiration.API.Features.SearchDestinations;

public static class SearchDestinations
{
    public static void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/destinations", async (string? searchFor,
            ILoggerFactory logger,
            CancellationToken cancellationToken,
            IDestinationSearchApiClient destinationSearchClient) =>
        {
            logger.CreateLogger("EndpointHandlers")
                .LogInformation("SearchDestionations feature called");

            var resultFromApiCall = await destinationSearchClient
                .GetDestinationsAsync(searchFor, cancellationToken);

            var result = resultFromApiCall.Select(x => new
            {
                x.Name,
                x.Description,
                x.ImageUri
            });

            return Results.Ok(result);
        });
    }
}
