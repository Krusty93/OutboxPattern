using System.Data;
using System.Reflection;
using System.Text.Json;
using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OutboxPattern.Domain.Notifications;
using Quartz;

namespace OutboxPattern.Infrastructure.Quartz
{
    [DisallowConcurrentExecution]
    public class ProcessOutboxJob : IJob
    {
        private readonly DapperContext _dapperContext;
        private readonly IMediator _mediator;
        private readonly ILogger<ProcessOutboxJob> _logger;

        public ProcessOutboxJob(
            DapperContext dapperContext,
            IMediator mediator,
            ILogger<ProcessOutboxJob> logger)
        {
            _dapperContext = dapperContext;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using IDbConnection connection = _dapperContext.CreateConnection();

            const string SQL_QUERY = "SELECT [Id], [Type], [Data] " +
                                     "FROM [dbo].[OutboxMessages] " +
                                     "WHERE [ProcessedDate] IS NULL";

            var messages = await connection.QueryAsync<OutboxMessageDto>(SQL_QUERY);

            foreach (var message in messages)
            {
                Type type = Assembly.GetAssembly(typeof(IDomainNotification<>)).GetType(message.Type);

                var notification = JsonSerializer.Deserialize(message.Data, type) as INotification;

                if (notification is null)
                {
                    _logger.LogWarning(
                        "Can't deserialize message {data} of type {type}",
                        message.Data,
                        message.Type);

                    continue;
                }

                await _mediator.Publish(notification);

                const string SQL_INSERT = "UPDATE [dbo].[OutboxMessages] " +
                                          "SET [ProcessedDate] = @Date " +
                                          "WHERE [Id] = @Id";

                await connection.ExecuteAsync(SQL_INSERT, new
                {
                    Date = DateTime.UtcNow,
                    message.Id
                });
            }
        }
    }
}
