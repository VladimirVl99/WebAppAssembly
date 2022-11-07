using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WebAppAssembly.Shared.Entities.CreateDelivery
{
    public class ComboInformation
    {
        // Created combo ID
        [JsonRequired]
        [JsonProperty("comboId")]
        [JsonPropertyName("comboId")]
        public Guid ComboId { get; set; }
        // Action ID that defines combo
        [JsonRequired]
        [JsonProperty("comboSourceId")]
        [JsonPropertyName("comboSourceId")]
        public Guid ComboSourceId { get; set; }
        // Combo group ID to which item belongs
        [JsonRequired]
        [JsonProperty("comboGroupId")]
        [JsonPropertyName("comboGroupId")]
        public Guid ComboGroupId { get; set; }
    }
}
