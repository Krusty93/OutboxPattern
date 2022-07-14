using System.Text.Json.Serialization;

namespace OutboxPattern.Application.Models
{
    public class AllOrdersResponse
    {
        public AllOrdersResponse(IEnumerable<OrderResponse> orders)
        {
            Orders = orders?.ToList() ?? throw new ArgumentNullException(nameof(orders));
        }

        [JsonPropertyName("orders")]
        public List<OrderResponse> Orders { get; set; }
    }

    public class OrderResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("total")]
        public decimal Total { get; set; }

        [JsonPropertyName("shippingAddress")]
        public string ShippingAddress { get; set; }
    }
}
