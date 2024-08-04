using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TravelInspiration.API.Shared.Domain.Entities;
using TravelInspiration.API.Shared.Domain.Events;
using TravelInspiration.API.Shared.Persistence;
using TravelInspiration.API.Shared.Slices;

namespace TravelInspiration.API.Features.Stops;

public sealed class CreateStop : ISlice
{
    public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost(
            "api/itineraries/{itineraryId}/stops",
            async (int itineraryId,
                CreateStopCommand request,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                request.ItineraryId = itineraryId;

                return await mediator.Send(request, cancellationToken);
            }
        );
    }

    public sealed class CreateStopCommand(int itineraryId,
        string name,
        string? imageUri) : IRequest<IResult>
    {
        public int ItineraryId { get; set; } = itineraryId;
        public string Name { get; } = name;
        public string? ImageUri { get; } = imageUri;
    }

    public sealed class CreateStopCommandValidator : AbstractValidator<CreateStopCommand>
    {
        public CreateStopCommandValidator()
        {
            RuleFor(v => v.Name)
                .MaximumLength(200)
                .NotEmpty();
            RuleFor(v => v.ImageUri)
                .Must(ImageUri => Uri.TryCreate(ImageUri ?? "", UriKind.Absolute, out _))
                .When(v => !string.IsNullOrEmpty(v.ImageUri))
                .WithMessage("ImageUri must be a valid Uri.");
        }
    }

    public sealed class CreateStopCommandHandler(TravelInspirationDbContext dbContext,
        IMapper mapper) : IRequestHandler<CreateStopCommand, IResult>
    {
        private readonly TravelInspirationDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;

        public async Task<IResult> Handle(CreateStopCommand request, CancellationToken cancellationToken)
        {
            var itineraryExists
                = await _dbContext.Itineraries
                .AsNoTracking()
                .AnyAsync(i => i.Id == request.ItineraryId, cancellationToken);

            if (!itineraryExists)
            {
                return Results.NotFound();
            }

            var stopEntity = new Stop(request.Name);
            stopEntity.HandleCreateCommand(request);

            _dbContext.Stops.Add(stopEntity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Results.Created($"api/itineraries/{stopEntity.ItineraryId}/stops/{stopEntity.Id}", 
                _mapper.Map<StopDto>(stopEntity));
        }

        public sealed class StopDto
        {
            public required int Id { get; set; }
            public required string Name { get; set; }
            public Uri? ImageUri { get; set; }
            public required int IteneraryId { get; set; }
        }

        public sealed class StopMapProfileAfterCreation : Profile
        {
            public StopMapProfileAfterCreation()
            {
                CreateMap<Stop, StopDto>();
            }
        }

        public sealed class SuggestStopStopCreatedEventHandler(
            ILogger<SuggestStopStopCreatedEventHandler> logger,
            TravelInspirationDbContext dbContext)
            : INotificationHandler<StopCreatedEvent>
        {
            private readonly ILogger<SuggestStopStopCreatedEventHandler> _logger = logger;
            private readonly TravelInspirationDbContext _dbContext = dbContext;

            public Task Handle(StopCreatedEvent notification, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Listiner {listiner} to domain event {domainEvent} triggered.",
                    GetType().Name,
                    notification.GetType().Name);

                var incomingStop = notification.Stop;

                var stop = new Stop($"AI-ified stop based on {incomingStop.Name}")
                {
                    ItineraryId = incomingStop.ItineraryId,
                    ImageUri = new Uri("https://picsum.photos/200/300"),
                    Suggested = true
                };

                _dbContext.Stops.Add(stop);
                //await _dbContext.SaveChangesAsync(cancellationToken);

                return Task.CompletedTask;
            }
        }

        public sealed class SuggestItineraryStopCreatedEventHandler(
            ILogger<SuggestStopStopCreatedEventHandler> logger)
            : INotificationHandler<StopCreatedEvent>
        {
            private readonly ILogger<SuggestStopStopCreatedEventHandler> _logger = logger;

            public Task Handle(StopCreatedEvent notification, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Listiner {listiner} to domain event {domainEvent} triggered.",
                   GetType().Name,
                   notification.GetType().Name);

                return Task.CompletedTask;
            }
        }
    }
}
