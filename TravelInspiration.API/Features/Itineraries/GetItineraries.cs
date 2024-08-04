using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TravelInspiration.API.Shared.Domain.Entities;
using TravelInspiration.API.Shared.Persistence;
using TravelInspiration.API.Shared.Slices;

namespace TravelInspiration.API.Features.Itineraries;

public sealed class GetItineraries : ISlice
{
    // "feature": "get-itineraries" (or ["", ""] for multiple features)

    private AuthorizationPolicy _hasGetItinerariesFaturePolicy
        => new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .RequireClaim("feature", "get-itineraries")
        .Build();

    public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("api/itineraries", async (string? searchFor, 
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            return await mediator.Send(new GetItinerariesQuery(searchFor), cancellationToken);
        }).RequireAuthorization(_hasGetItinerariesFaturePolicy);
    }

    public sealed class GetItinerariesQuery(string? searchFor) : IRequest<IResult>
    {
        public string? SearchFor { get; } = searchFor;
    }

    public class GetItinerariesQueryHandler(TravelInspirationDbContext dbContext, IMapper mapper)
        : IRequestHandler<GetItinerariesQuery, IResult>
    {
        private readonly TravelInspirationDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;

        public async Task<IResult> Handle(GetItinerariesQuery request, 
            CancellationToken cancellationToken)
        {
            var itineraries = await _dbContext.Itineraries
                .AsNoTracking()
                .Where(x => request.SearchFor == null
                    || x.Name.Contains(request.SearchFor)
                    || (x.Description != null && x.Description.Contains(request.SearchFor)))
                .ToListAsync(cancellationToken);

            return Results.Ok(_mapper.Map<IEnumerable<ItineraryDto>>(itineraries));
        }
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
