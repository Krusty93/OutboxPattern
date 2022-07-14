using System.Text.Json.Serialization;
using MediatR;

namespace OutboxPattern.Application.Commands
{
    public class CreateOrderCommand : IRequest<Guid>
    {
        [JsonConstructor]
        public CreateOrderCommand(
            string shippingAddress,
            Guid productId,
            int quantity)
        {
            ShippingAddress = shippingAddress;
            ProductId = productId;
            Quantity = quantity;
        }

        [JsonPropertyName("shippingAddress")]
        public string ShippingAddress { get; }

        [JsonPropertyName("productId")]
        public Guid ProductId { get; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; }
    }
}
