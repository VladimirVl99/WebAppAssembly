using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace ApiServerForTelegram.Entities.IikoCloudApi.General.Menu.RetrieveExternalMenuByID
{
    public class ModifierRestrictionsDto
    {
        [JsonProperty("minQuantity")]
        [JsonPropertyName("minQuantity")]
        public int? MinQuantity { get; set; }
        [JsonProperty("maxQuantity")]
        [JsonPropertyName("maxQuantity")]
        public int? MaxQuantity { get; set; }
        [JsonProperty("freeQuantity")]
        [JsonPropertyName("freeQuantity")]
        public int? FreeQuantity { get; set; }
        [JsonProperty("byDefault")]
        [JsonPropertyName("byDefault")]
        public int? ByDefault { get; set; }
    }
}
