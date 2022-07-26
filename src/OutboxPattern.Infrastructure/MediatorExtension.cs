using System.Reflection;
using System.Text.Json;
using MediatR;
using OutboxPattern.Domain.Events;
using OutboxPattern.Domain.Models;
using OutboxPattern.Domain.Notifications;
using OutboxPattern.Infrastructure.Processing;

namespace OutboxPattern.Infrastructure
{
    internal static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync(
            this IMediator mediator,
            OutboxDbContext dbContext)
        {
            var domainEntities = dbContext.ChangeTracker
                .Entries<DomainEventsBasedObject>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            var domainEventNotifications = new List<IDomainNotification<IDomainEvent>>();

            foreach (INotification domainEvent in domainEvents)
            {
                Type domainEvenNotificationType = typeof(IDomainNotification<>);

                Type domainNotificationWithGenericType = domainEvenNotificationType.MakeGenericType(domainEvent.GetType());

                var assembly = Assembly.Load("OutboxPattern.Domain");
                var notificationTypeName = domainEvent.GetType().FullName.Replace("Event", "Notification");
                Type notificationType = assembly.GetType(notificationTypeName);

                object domainNotification = Activator.CreateInstance(type: notificationType, args: domainEvent);

                if (domainNotification != null)
                {
                    domainEventNotifications.Add(domainNotification as IDomainNotification<IDomainEvent>);
                }
            }

            domainEntities
                .ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await mediator.Publish(domainEvent);
                });

            await Task.WhenAll(tasks);

            foreach (IDomainNotification<IDomainEvent> domainEventNotification in domainEventNotifications)
            {
                string type = domainEventNotification.GetType().FullName;

                // DO NOT FORGET TO SERIALIZE AS OBJECT
                var data = JsonSerializer.Serialize<object>(domainEventNotification);

                OutboxMessage outboxMessage = OutboxMessage.Create(
                    domainEventNotification.DomainEvent.OccurredOn,
                    type,
                    data);

                dbContext.OutboxMessages.Add(outboxMessage);
            }
        }
    }
}
