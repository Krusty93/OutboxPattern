using System.Text.Json.Serialization;
using OutboxPattern.Domain.Events;

namespace OutboxPattern.Domain.Notifications
{
    public class BaseDomainNotification<TDomainEvent>
        : IDomainNotification<TDomainEvent> where TDomainEvent : IDomainEvent
    {
        public BaseDomainNotification(TDomainEvent domainEvent)
        {
            Id = Guid.NewGuid();
            DomainEvent = domainEvent;
        }

        [JsonIgnore]
        public TDomainEvent DomainEvent { get; }

        public Guid Id { get; }
    }
}
