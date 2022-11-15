using Newtonsoft.Json;
using System.Text.Json.Serialization;
using WebAppAssembly.Shared.Entities.EMenu;

namespace ApiServerForTelegram.Entities.IikoCloudApi.General.Menu.RetrieveExternalMenuByID
{
    public class TransportItemDto
    {
        [JsonProperty("itemSizes")]
        [JsonPropertyName("itemSizes")]
        public IEnumerable<TransportItemSizeDto>? ItemSizes { get; set; }
        [JsonProperty("sku")]
        [JsonPropertyName("sku")]
        public string? Sku { get; set; }
        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonProperty("description")]
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonProperty("allergenGroups")]
        [JsonPropertyName("allergenGroups")]
        public IEnumerable<AllergenGroupDto>? AllergenGroups { get; set; }
        [JsonProperty("iikoItemId")]
        [JsonPropertyName("iikoItemId")]
        public Guid? ItemId { get; set; }
        [JsonProperty("iikoModifierSchemaId")]
        [JsonPropertyName("iikoModifierSchemaId")]
        public Guid? ModifierSchemaId { get; set; }
        [JsonProperty("taxCategory")]
        [JsonPropertyName("taxCategory")]
        public IEnumerable<TaxCategoryDto>? TaxCategories { get; set; }
        [JsonProperty("orderItemType")]
        [JsonPropertyName("orderItemType")]
        public string? OrderItemType { get; set; }
        [JsonProperty("tags")]
        [JsonPropertyName("tags")]
        public IEnumerable<InternalTag>? InternalTags { get; set; }
        [JsonProperty("labeles")]
        [JsonPropertyName("labeles")]
        public IEnumerable<ExternalTag>? ExternalTags { get; set; }
        [JsonProperty("iikoModifierSchemaName")]
        [JsonPropertyName("iikoModifierSchemaName")]
        public string? ModifierSchemaName { get; set; }
        [JsonProperty("totalAmount")]
        [JsonPropertyName("totalAmount")]
        public int TotalAmount { get; set; } = 0;



        public string ImageLink() => ItemSizes?.LastOrDefault()?.ButtonImageUrl ?? string.Empty;
        public bool HaveModifiers()
        {
            var modifierGroups = ItemSizes?.LastOrDefault()?.ItemModifierGroups;
            return modifierGroups is not null && modifierGroups.Any();
        }
        public double? Price() => ItemSizes?.LastOrDefault()?.Prices?.LastOrDefault()?.Price;
        public bool HaveItems() => TotalAmount > 0;
        public void IncrementAmount() => TotalAmount++;
        public void DecrementAmount() => TotalAmount = TotalAmount != 0 ? --TotalAmount : TotalAmount;
        public float Weight() => ItemSizes?.LastOrDefault()?.PortionWeightGrams ?? 0;
        public float Fats() => ItemSizes?.LastOrDefault()?.NutritionPerHundredGrams?.Fats ?? 0;
        public float Proteins() => ItemSizes?.LastOrDefault()?.NutritionPerHundredGrams?.Proteins ?? 0;
        public float Carbs() => ItemSizes?.LastOrDefault()?.NutritionPerHundredGrams?.Carbs ?? 0;
        public float Energy() => ItemSizes?.LastOrDefault()?.NutritionPerHundredGrams?.Energy ?? 0;
        public IEnumerable<TransportModifierGroupDto>? ModifierGroups() => ItemSizes?.LastOrDefault()?.ItemModifierGroups;
    }
}
