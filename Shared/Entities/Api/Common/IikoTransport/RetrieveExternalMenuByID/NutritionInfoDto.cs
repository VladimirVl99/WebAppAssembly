using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.RetrieveExternalMenuByID
{
    public class NutritionInfoDto
    {
        [JsonProperty("fats")]
        [JsonPropertyName("fats")]
        public float? Fats { get; set; }
        [JsonProperty("proteins")]
        [JsonPropertyName("proteins")]
        public float? Proteins { get; set; }
        [JsonProperty("carbs")]
        [JsonPropertyName("carbs")]
        public float? Carbs { get; set; }
        [JsonProperty("energy")]
        [JsonPropertyName("energy")]
        public float? Energy { get; set; }
        [JsonProperty("organizations")]
        [JsonPropertyName("organizations")]
        public IEnumerable<Guid>? Organizations { get; set; }
    }
}
