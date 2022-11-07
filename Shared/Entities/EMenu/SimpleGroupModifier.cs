using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WebAppAssembly.Shared.Entities.EMenu
{
    public class SimpleGroupModifier
    {
        public SimpleGroupModifier(Guid id, string name, double minAmount, double maxAmount)
        {
            Id = id;
            MinAmount = minAmount;
            MaxAmount = maxAmount;
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
        public double MinAmount { get; set; }
        [JsonProperty("maxAmount")]
        [JsonPropertyName("maxAmount")]
        public double MaxAmount { get; set; }
    }
}
