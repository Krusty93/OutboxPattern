using MediatR;

namespace OutboxPattern.Domain.Models
{
    public abstract class DomainEventsBasedObject
    {
        private readonly List<INotification> _domainEvents = new();

        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        public async Task RaiseDomainEventsAsync(IMediator mediator)
        {
            var domainEvents = DomainEvents.ToList();

            ClearDomainEvents();

            IEnumerable<Task> tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await mediator.Publish(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}
