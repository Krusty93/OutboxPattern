using System.Text.Json.Serialization;
using MediatR;

namespace OutboxPattern.Application.Commands
{
    public class DeleteOrderCommand : IRequest
    {
        [JsonConstructor]
        public DeleteOrderCommand(Guid orderId)
        {
            OrderId = orderId;
        }

        [JsonPropertyName("orderId")]
        public Guid OrderId { get; }
    }
}
