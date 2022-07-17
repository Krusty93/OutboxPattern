namespace OutboxPattern.Domain.Events
{
    public class OrderPlacedDomainEvent : BaseDomainEvent
    {
        public OrderPlacedDomainEvent(
            Guid orderId,
            decimal price,
            string shippingAddress,
            Guid productId)
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
