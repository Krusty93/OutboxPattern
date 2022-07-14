using System.Text.Json.Serialization;
using MediatR;

namespace OutboxPattern.Application.Commands
{
    public class DeleteProductCommand : IRequest
    {
        [JsonConstructor]
        public DeleteProductCommand(Guid productId)
        {
            ProductId = productId;
        }

        [JsonPropertyName("productId")]
        public Guid ProductId { get; }
    }
}
