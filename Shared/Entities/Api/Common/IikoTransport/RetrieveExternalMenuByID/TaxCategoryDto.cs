using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.RetrieveExternalMenuByID
{
    public class TaxCategoryDto
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonProperty("percentage")]
        [JsonPropertyName("percentage")]
        public float? Percentage { get; set; }
    }
}
