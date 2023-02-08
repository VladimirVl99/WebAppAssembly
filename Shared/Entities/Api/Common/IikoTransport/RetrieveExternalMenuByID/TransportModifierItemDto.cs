using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.RetrieveExternalMenuByID
{
    public class TransportModifierItemDto
    {
        [JsonProperty("prices")]
        [JsonPropertyName("prices")]
        public IEnumerable<TransportPriceDto>? Prices { get; set; }
        [JsonProperty("sku")]
        [JsonPropertyName("sku")]
        public string? Sku { get; set; }
        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonProperty("description")]
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonProperty("buttonImage")]
        [JsonPropertyName("buttonImage")]
        public string? ButtonImage { get; set; }
        [JsonProperty("restrictions")]
        [JsonPropertyName("restrictions")]
        public ModifierRestrictionsDto? Restrictions { get; set; }
        [JsonProperty("allergenGroups")]
        [JsonPropertyName("allergenGroups")]
        public IEnumerable<AllergenGroupDto>? AllergenGroups { get; set; }
        [JsonProperty("nutritionPerHundredGrams")]
        [JsonPropertyName("nutritionPerHundredGrams")]
        public NutritionInfoDto? NutritionPerHundredGrams { get; set; }
        [JsonProperty("nutritions")]
        [JsonPropertyName("nutritions")]
        public IEnumerable<NutritionInfoDto>? Nutritions { get; set; }
        [JsonProperty("portionWeightGrams")]
        [JsonPropertyName("portionWeightGrams")]
        public float? PortionWeightGrams { get; set; }
        [JsonProperty("tags")]
        [JsonPropertyName("tags")]
        public IEnumerable<InternalTag>? InternalTags { get; set; }
        [JsonProperty("labels")]
        [JsonPropertyName("labels")]
        public IEnumerable<ExternalTag>? ExternalTags { get; set; }
        [JsonProperty("itemId")]
        [JsonPropertyName("itemId")]
        public Guid? ItemId { get; set; }

        public double Price() => Prices?.FirstOrDefault()?.Price ?? 0;
    }
}
