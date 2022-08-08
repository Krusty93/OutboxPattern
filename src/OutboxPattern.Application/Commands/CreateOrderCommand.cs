using System.Text.Json.Serialization;
using MediatR;

namespace OutboxPattern.Application.Commands
{
    public class CreateOrderCommand : IRequest<Guid>
    {
        [JsonConstructor]
        public CreateOrderCommand(
            ShippingAddressDto shippingAddress,
            Guid productId,
            int quantity)
        {
            ShippingAddress = shippingAddress;
            ProductId = productId;
            Quantity = quantity;
        }

        [JsonPropertyName("shippingAddress")]
        public ShippingAddressDto ShippingAddress { get; }

        [JsonPropertyName("productId")]
        public Guid ProductId { get; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; }
    }

    public class ShippingAddressDto
    {
        [JsonPropertyName("street")]
        public string Street { get; set; }

        [JsonPropertyName("number")]
        public int Number { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        public override string ToString() =>
            $"{Street} {Number}, {City}";
    }
}
