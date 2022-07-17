using MediatR;

namespace OutboxPattern.Domain.Notifications
{
    public interface IDomainNotification<out TDomainEvent> : IDomainNotification
    {
        public TDomainEvent DomainEvent { get; }
    }

    public interface IDomainNotification : INotification
    {
        public Guid Id { get; }
    }
}
