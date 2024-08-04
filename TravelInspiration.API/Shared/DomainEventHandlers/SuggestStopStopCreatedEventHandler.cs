using MediatR;
using TravelInspiration.API.Shared.Domain.Entities;
using TravelInspiration.API.Shared.Domain.Events;
using TravelInspiration.API.Shared.Persistence;

namespace TravelInspiration.API.Shared.DomainEventHandlers;

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

        return Task.CompletedTask;
    }
}
