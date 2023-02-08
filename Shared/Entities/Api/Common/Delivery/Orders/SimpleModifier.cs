using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WebAppAssembly.Shared.Entities.Api.Common.Delivery.Orders
{
    public class SimpleModifier
    {
        public SimpleModifier(Guid id, string name, double minAmount, double maxAmount)
        {
            Id = id;
            MinQuantity = minAmount;
            MaxQuantity = maxAmount;
            Name = name;
        }

        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonProperty("minAmount")]
        [JsonPropertyName("minAmount")]
        public double MinQuantity { get; set; }
        [JsonProperty("maxAmount")]
        [JsonPropertyName("maxAmount")]
        public double MaxQuantity { get; set; }
    }
}
