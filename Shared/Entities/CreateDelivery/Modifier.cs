using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WebAppAssembly.Shared.Entities.CreateDelivery
{
    public class Modifier
    {
        public Modifier(Guid productId, string name, Guid? productGroupId = default, double? minAmount = default, double? maxAmount = default, double defaultAmount = 0,
            double? price = null)
        {
            ProductId = productId;
            ProductGroupId = productGroupId;
            MinAmount = minAmount;
            MaxAmount = maxAmount;
            Amount = defaultAmount;
            Price = price;
            Name = name;
        }

        // Modifier item ID
        // Can be obtained by /api/1/nomenclature operation
        [JsonRequired]
        [JsonProperty("productId")]
        [JsonPropertyName("productId")]
        public Guid ProductId { get; set; }
        // Quantity
        [JsonRequired]
        [JsonProperty("amount")]
        [JsonPropertyName("amount")]
        public double Amount { get; set; } = 0;
        // Modifiers group ID (for group modifier). Required for a group modifier
        // Can be obtained by /api/1/nomenclature operation
        [JsonProperty("productGroupId")]
        [JsonPropertyName("productGroupId")]
        public Guid? ProductGroupId { get; set; }
        // Unit price
        [JsonProperty("price")]
        [JsonPropertyName("price")]
        public double? Price { get; set; }
        // Unique identifier of the item in the order. MUST be unique for the whole system. Therefore it must be generated with Guid.NewGuid()
        // If sent null, it generates automatically on iikoTransport side
        [JsonProperty("positionId")]
        [JsonPropertyName("positionId")]
        public Guid? PositionId { get; set; }
        [JsonProperty("minAmount")]
        [JsonPropertyName("minAmount")]
        public double? MinAmount { get; set; }
        [JsonProperty("maxAmount")]
        [JsonPropertyName("maxAmount")]
        public double? MaxAmount { get; set; }
        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}