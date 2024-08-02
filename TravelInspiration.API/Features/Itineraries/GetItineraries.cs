using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TravelInspiration.API.Shared.Domain.Entities;
using TravelInspiration.API.Shared.Persistence;
using TravelInspiration.API.Shared.Slices;

namespace TravelInspiration.API.Features.Itineraries;

public sealed class GetItineraries : ISlice
{
    public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("api/itineraries", async (string? searchFor, 
            ILoggerFactory loggerFactory,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            loggerFactory.CreateLogger("EndpointHandlers")
                .LogInformation("GetItineraries feature called");

            return await mediator.Send(new GetItinerariesQuery(searchFor), cancellationToken);
        });
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
