using System.Text.Json.Serialization;
using MediatR;

namespace OutboxPattern.Application.Commands
{
    public class CreateProductCommand : IRequest<Guid>
    {
        [JsonConstructor]
        public CreateProductCommand(string name, decimal price, int quantity)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
        }

        [JsonPropertyName("name")]
        public string Name { get; }

        [JsonPropertyName("price")]
        public decimal Price { get; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; }
    }
}
