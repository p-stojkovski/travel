using MediatR;

namespace TravelInspiration.API.Shared.Domain;

public abstract class DomainEvent : INotification
{
    public bool IsPublished { get; }
    public DateTimeOffset OccurredOn { get; } = DateTime.UtcNow;
}
