using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TravelInspiration.API.Shared.Domain.Entities;
using TravelInspiration.API.Shared.Persistence;

namespace TravelInspiration.API.Features.Itineraries;

public static class GetItineraries
{
    public static void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/itineraries", async (string? searchFor, 
            ILoggerFactory loggerFactory, 
            TravelInspirationDbContext dbContext,
            IMapper mapper,
            CancellationToken cancellationToken) =>
        {
            loggerFactory.CreateLogger("EndpointHandlers")
                .LogInformation("GetItineraries feature called");

            var result = await dbContext.Itineraries
                .Where(x => searchFor == null 
                    || x.Name.Contains(searchFor)
                    || (x.Description != null && x.Description.Contains(searchFor)))
                .ToListAsync(cancellationToken);

            return Results.Ok(mapper.Map<IEnumerable<ItineraryDto>>(result));
        });
    }

    public sealed class ItineraryDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string UserId { get; set; }
    }

    public class ItineraryMapProfile : Profile
    {
        public ItineraryMapProfile()
        {
            CreateMap<Itinerary, ItineraryDto>();
        }
    }
}
