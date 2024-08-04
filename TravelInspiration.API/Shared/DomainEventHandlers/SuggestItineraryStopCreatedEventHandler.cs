using MediatR;
using TravelInspiration.API.Shared.Domain.Events;

namespace TravelInspiration.API.Shared.DomainEventHandlers;

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
