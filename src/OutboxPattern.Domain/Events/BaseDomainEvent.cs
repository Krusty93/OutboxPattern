using MediatR;

namespace OutboxPattern.Domain.Events
{
    public abstract class BaseDomainEvent : IDomainEvent
    {
        protected BaseDomainEvent()
        {
            OccurredOn = DateTime.UtcNow;
        }

        public DateTime OccurredOn { get; }
    }

    public interface IDomainEvent : INotification
    {
        DateTime OccurredOn { get; }
    }
}
