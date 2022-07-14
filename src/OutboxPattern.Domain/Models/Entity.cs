namespace OutboxPattern.Domain.Models
{
    public abstract class Entity<TKey> : DomainEventsBasedObject
    {
        public virtual TKey Id { get; protected set; }
    }
}
