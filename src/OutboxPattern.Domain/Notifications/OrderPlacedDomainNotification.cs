using System.Text.Json.Serialization;
using OutboxPattern.Domain.Events;

namespace OutboxPattern.Domain.Notifications
{
    public class OrderPlacedDomainNotification : BaseDomainNotification<OrderPlacedDomainEvent>
    {
        public OrderPlacedDomainNotification(OrderPlacedDomainEvent domainEvent)
            : base(domainEvent)
        {
            OrderId = domainEvent.OrderId;
            Price = domainEvent.Price;
            ShippingAddress = domainEvent.ShippingAddress;
            ProductId = domainEvent.ProductId;
        }

        [JsonConstructor]
        public OrderPlacedDomainNotification(
            Guid orderId,
            decimal price,
            string shippingAddress,
            Guid productId)
            : base(null)
        {
            OrderId = orderId;
            Price = price;
            ShippingAddress = shippingAddress;
            ProductId = productId;
        }

        public Guid OrderId { get; }

        public decimal Price { get; }

        public string ShippingAddress { get; }

        public Guid ProductId { get; }
    }
}
