using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WebAppAssembly.Shared.Models.Order
{
    public class ProductModel
    {
        [JsonProperty("productId")]
        [JsonPropertyName("productId")]
        public Guid ProductId { get; set; }
        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonProperty("imageUrl")]
        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; set; } = string.Empty;
        [JsonProperty("amount")]
        [JsonPropertyName("amount")]
        public int Amount { get; set; }
        [JsonProperty("price")]
        [JsonPropertyName("price")]
        public double Price { get; set; }
    }
}
