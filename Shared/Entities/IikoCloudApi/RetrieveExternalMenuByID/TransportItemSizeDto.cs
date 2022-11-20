using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace ApiServerForTelegram.Entities.IikoCloudApi.General.Menu.RetrieveExternalMenuByID
{
    public class TransportItemSizeDto
    {
        [JsonProperty("prices")]
        [JsonPropertyName("prices")]
        public IEnumerable<TransportPriceDto>? Prices { get; set; }
        [JsonProperty("itemModifierGroups")]
        [JsonPropertyName("itemModifierGroups")]
        public IEnumerable<TransportModifierGroupDto>? ItemModifierGroups { get; set; }
        [JsonProperty("sku")]
        [JsonPropertyName("sku")]
        public string? Sku { get; set; }
        [JsonProperty("sizeCode")]
        [JsonPropertyName("sizeCode")]
        public string? SizeCode { get; set; }
        [JsonProperty("sizeName")]
        [JsonPropertyName("sizeName")]
        public string? SizeName { get; set; }
        [JsonProperty("isDefault")]
        [JsonPropertyName("isDefault")]
        public bool IsDefault { get; set; }
        [JsonProperty("portionWeightGrams")]
        [JsonPropertyName("portionWeightGrams")]
        public float? PortionWeightGrams { get; set; }
        [JsonProperty("sizeId")]
        [JsonPropertyName("sizeId")]
        public Guid? SizeId { get; set; }
        [JsonProperty("nutritionPerHundredGrams")]
        [JsonPropertyName("nutritionPerHundredGrams")]
        public NutritionInfoDto? NutritionPerHundredGrams { get; set; }
        [JsonProperty("nutritions")]
        [JsonPropertyName("nutritions")]
        public IEnumerable<NutritionInfoDto>? Nutritions { get; set; }
        [JsonProperty("buttonImageUrl")]
        [JsonPropertyName("buttonImageUrl")]
        public string? ButtonImageUrl { get; set; }
        [JsonProperty("buttonImageCroppedUrl")]
        [JsonPropertyName("buttonImageCroppedUrl")]
        public IEnumerable<string>? ButtonImageCroppedUrl { get; set; }
    }
}
