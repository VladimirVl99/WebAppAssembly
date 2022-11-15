using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace ApiServerForTelegram.Entities.IikoCloudApi.General.Menu.RetrieveExternalMenuByID
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
