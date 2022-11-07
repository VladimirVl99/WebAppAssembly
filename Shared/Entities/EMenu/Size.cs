using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WebAppAssembly.Shared.Entities.EMenu
{
    public class Size
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonProperty("priority")]
        [JsonPropertyName("priority")]
        public int? Priority { get; set; }
        [JsonProperty("isDefault")]
        [JsonPropertyName("isDefault")]
        public bool IsDefault { get; set; }
    }
}
