using System.Text.Json.Serialization;

namespace OutboxPattern.Application.Models
{
    public class AllProductResponse
    {
        public AllProductResponse(IEnumerable<ProductResponse> orders)
        {
            Products = orders?.ToList() ?? throw new ArgumentNullException(nameof(orders));
        }

        [JsonPropertyName("products")]
        public List<ProductResponse> Products { get; set; }
    }

    public class ProductResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
    }
}
