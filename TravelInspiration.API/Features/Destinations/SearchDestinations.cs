using TravelInspiration.API.Shared.Networking;
using TravelInspiration.API.Shared.Slices;

namespace TravelInspiration.API.Features.Destinations;

public sealed class SearchDestinations : ISlice
{
    public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("api/destinations", async (string? searchFor,
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
