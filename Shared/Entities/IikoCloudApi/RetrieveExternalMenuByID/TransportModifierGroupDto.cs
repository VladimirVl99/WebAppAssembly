using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace ApiServerForTelegram.Entities.IikoCloudApi.General.Menu.RetrieveExternalMenuByID
{
    public class TransportModifierGroupDto
    {
        [JsonProperty("items")]
        [JsonPropertyName("items")]
        public IEnumerable<TransportModifierItemDto>? Items { get; set; }
        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonProperty("description")]
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonProperty("restrictions")]
        [JsonPropertyName("restrictions")]
        public ModifierRestrictionsDto? Restrictions { get; set; }
        [JsonProperty("canBeDivided")]
        [JsonPropertyName("canBeDivided")]
        public bool CanBeDivided { get; set; }
        [JsonProperty("iikoItemGroupId")]
        [JsonPropertyName("iikoItemGroupId")]
        public Guid? ItemGroupId { get; set; }
        [JsonProperty("childModifiersHaveMinMaxRestrictions")]
        [JsonPropertyName("childModifiersHaveMinMaxRestrictions")]
        public bool ChildModifiersHaveMinMaxRestrictions { get; set; }
        [JsonProperty("sku")]
        [JsonPropertyName("sku")]
        public string? Sku { get; set; }
    }
}
