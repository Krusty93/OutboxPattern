using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using OutboxPattern.FunctionApp.Helpers;

namespace OutboxPattern.FunctionApp
{
#pragma warning disable IDE0008 // Use explicit type
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task RunAsync(
            [EventHubTrigger(
                "wwi",
                Connection = "Debezium")] EventData eventData,
            [ServiceBus("outbox", Connection = "ServiceBusConnection")] ICollector<ServiceBusMessage> outputQueueItem,
            [Sql("dbo.OutboxMessages", ConnectionStringSetting = "SqlConnectionString")] IAsyncCollector<OutboxMessageDomain> outboxMessages,
            ILogger log)
        {
            var exceptions = new List<Exception>();

            {
                log.LogInformation("Processing event {id}", eventData.MessageId);

                try
                {
                    await ReadEventHubMessageAsync(eventData, outputQueueItem, outboxMessages);
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }
            }

            if (exceptions.Count > 1)
                log.LogError(exceptions.Single(), "Error");
        }

        private static async Task ReadEventHubMessageAsync(
            EventData eventData,
            ICollector<ServiceBusMessage> outputQueueItem,
            IAsyncCollector<OutboxMessageDomain> outboxMessages)
        {
            ServiceBusMessage serviceBusMessage;

            var message = JsonSerializer.Deserialize<OutboxMessage>(eventData.EventBody);

            OutboxMessageDomain domain = null;

            switch (message.Operation)
            {
                case "c":
                    var body = JsonSerializer.Deserialize<EventHubOutboxMessage>(eventData.EventBody);
                    if (body.After is null)
                        return;

                    var newRowStatus = body.After;

                    domain = new OutboxMessageDomain(
                        Id: newRowStatus.Id,
                        OccurredOn: (newRowStatus.OccurredOn / 1000000000).AsUnixTimestamp(),
                        Type: newRowStatus.Type,
                        Data: newRowStatus.Data,
                        ProcessedDate: DateTime.UtcNow);

                    serviceBusMessage = ProcessInsertOperation(domain);
                    break;
                case "u":
                    serviceBusMessage = ProcessUpdateOperation();
                    break;
                case "d":
                    serviceBusMessage = ProcessDeleteOperation();
                    break;
                default:
                    return;
            }

            if (serviceBusMessage is not null)
                outputQueueItem.Add(serviceBusMessage);

            if (domain is not null)
                await outboxMessages.AddAsync(domain);
        }

        private static ServiceBusMessage ProcessInsertOperation(
            OutboxMessageDomain newOutboxMessage)
        {
            var message = new ServiceBusMessage(body: newOutboxMessage.Data)
            {
                Subject = newOutboxMessage.Type,
            };

            message.ApplicationProperties.Add("OutboxId", newOutboxMessage.Id);

            return message;
        }

        private static ServiceBusMessage ProcessUpdateOperation()
        {
            return null;
        }

        private static ServiceBusMessage ProcessDeleteOperation()
        {
            return null;
        }
    }
}
