using System.Net.Mail;
using MediatR;
using Microsoft.Extensions.Configuration;
using OutboxPattern.Domain.Notifications;

namespace OutboxPattern.Application.NotificationHandlers
{
    internal class OrderPlacedDomainNotificationHandler : INotificationHandler<OrderPlacedDomainNotification>
    {
        private readonly IConfiguration _configuration;

        public OrderPlacedDomainNotificationHandler(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Handle(OrderPlacedDomainNotification notification, CancellationToken cancellationToken)
        {
            var fromAddress = _configuration["EmailSettings:FromAddress"];
            var recipient = _configuration["EmailSettings:ToAddress"];
            var host = _configuration["EmailSettings:Host"];
            var port = _configuration["EmailSettings:Port"];

            using var smtpClient = new SmtpClient
            {
                Host = host,
                Port = int.Parse(port),
            };

            using var message = new MailMessage(fromAddress, recipient)
            {
                Subject = $"A new order has been submitted: #{notification.OrderId}",
                Body = $"Created a new order for product {notification.ProductId} for a total of {notification.Price}€",
                IsBodyHtml = false,
            };

            await smtpClient.SendMailAsync(message, cancellationToken);
        }
    }
}
