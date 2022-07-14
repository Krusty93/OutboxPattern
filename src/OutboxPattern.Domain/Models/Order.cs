using OutboxPattern.Domain.Events;

namespace OutboxPattern.Domain.Models
{
    public class Order : Entity<Guid>, IAggregateRoot
    {
        /// <summary>
        /// EF reserved
        /// </summary>
        protected Order()
        {

        }

        private Order(
            decimal total,
            string shippingAddress,
            Guid productId)
        {
            Total = total;
            ShippingAddress = shippingAddress;
            ProductId = productId;
        }

        public decimal Total { get; private set; }

        public string ShippingAddress { get; private set; }

        public Guid ProductId { get; private set; }

        public static Order Create(decimal unitPrice, int quantity, string shippingAddress, Guid productId)
        {
            decimal total = unitPrice * quantity;

            return new Order(total, shippingAddress, productId);
        }

        public void ExecuteOrder()
        {
            AddDomainEvent(new OrderPlacedDomainEvent(Id, Total, ShippingAddress, ProductId));
        }
    }
}
