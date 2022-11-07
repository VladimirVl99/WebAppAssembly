using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebAppAssembly.Shared.Entities.EMenu
{
    public class Price
    {
        [Required]
        [JsonProperty("currentPrice")]
        [JsonPropertyName("currentPrice")]
        public double CurrentPrice { get; set; }
        [Required]
        [JsonProperty("isIncludedInMenu")]
        [JsonPropertyName("isIncludedInMenu")]
        public bool IsIncludedInMenu { get; set; }
        [JsonProperty("nextPrice")]
        [JsonPropertyName("nextPrice")]
        public double? NextPrice { get; set; }
        [Required]
        [JsonProperty("nextIncludedInMenu")]
        [JsonPropertyName("nextIncludedInMenu")]
        public bool NextIncludedInMenu { get; set; }
        [JsonProperty("nextDatePrice")]
        [JsonPropertyName("nextDatePrice")]
        public string? NextDatePrice { get; set; }
    }
}
