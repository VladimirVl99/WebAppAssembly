using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace ApiServerForTelegram.Entities.IikoCloudApi.General.Menu.RetrieveExternalMenuByID
{
    public class TransportMenuCategoryDto
    {
        [JsonProperty("items")]
        [JsonPropertyName("items")]
        public IEnumerable<TransportItemDto>? Items { get; set; }
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonProperty("description")]
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonProperty("buttonImageUrl")]
        [JsonPropertyName("buttonImageUrl")]
        public string? ButtonImageUrl { get; set; }
        [JsonProperty("headerImageUrl")]
        [JsonPropertyName("headerImageUrl")]
        public string? РeaderImageUrl { get; set; }
        [JsonProperty("iikoGroupId")]
        [JsonPropertyName("iikoGroupId")]
        public Guid? IikoGroupId { get; set; }
    }
}
