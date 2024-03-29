﻿using Newtonsoft.Json;
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
        [JsonProperty("itemId")]
        [JsonPropertyName("itemId")]
        public Guid? ItemId { get; set; }
        [JsonProperty("modifierSchemaId")]
        [JsonPropertyName("modifierSchemaId")]
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
        [JsonProperty("modifierSchemaName")]
        [JsonPropertyName("modifierSchemaName")]
        public string? ModifierSchemaName { get; set; }
        [JsonProperty("totalAmount")]
        [JsonPropertyName("totalAmount")]
        public int TotalAmount { get; set; } = 0;



        public string ImageLink() => ItemSizes?.FirstOrDefault()?.ButtonImageUrl ?? string.Empty;
        public bool HaveModifiers()
        {
            var modifierGroups = ItemSizes?.FirstOrDefault()?.ItemModifierGroups;
            return modifierGroups is not null && modifierGroups.Any();
        }
        public bool HaveSizesMoreThanOne()
        { 
            if (ItemSizes is null) return false;
            int i = 0;
            foreach (var size in ItemSizes)
            {
                if (i > 0) return true;
                i++;
            }
            return false;
        }
        public float? Price(Guid? sizeId = null) => 
            sizeId is null ? ItemSizes?.FirstOrDefault()?.Prices?.FirstOrDefault()?.Price : ItemSizes?.FirstOrDefault(x => x.SizeId == sizeId)?.Prices?.FirstOrDefault()?.Price;
        public bool HaveItems() => TotalAmount > 0;
        public void IncrementAmount() => TotalAmount++;
        public void DecrementAmount() => TotalAmount = TotalAmount != 0 ? --TotalAmount : TotalAmount;
        private static string IntOrSomeNumberOfDigitsFromCurrentCulture(float number, int numberOfDigitsFromCurrentCulture) =>
            ((int)(number * 100) % 100) != 0 ? string.Format($"{{0:F{numberOfDigitsFromCurrentCulture}}}", number) : ((int)number).ToString();
        public float Weight() => ItemSizes?.FirstOrDefault()?.PortionWeightGrams ?? 0;
        public string WeightAsString() => IntOrSomeNumberOfDigitsFromCurrentCulture(Weight(), 2);
        public float Fats() => ItemSizes?.FirstOrDefault()?.NutritionPerHundredGrams?.Fats ?? 0;
        public string FatsAsString() => IntOrSomeNumberOfDigitsFromCurrentCulture(Fats(), 2);
        public float Proteins() => ItemSizes?.FirstOrDefault()?.NutritionPerHundredGrams?.Proteins ?? 0;
        public string ProteinsAsString() => IntOrSomeNumberOfDigitsFromCurrentCulture(Proteins(), 2);
        public float Carbs() => ItemSizes?.FirstOrDefault()?.NutritionPerHundredGrams?.Carbs ?? 0;
        public string CarbsAsString() => IntOrSomeNumberOfDigitsFromCurrentCulture(Carbs(), 2);
        public float Energy() => ItemSizes?.FirstOrDefault()?.NutritionPerHundredGrams?.Energy ?? 0;
        public string EnergyAsString() => IntOrSomeNumberOfDigitsFromCurrentCulture(Energy(), 2);
        public IEnumerable<TransportModifierGroupDto>? ModifierGroups() => ItemSizes?.FirstOrDefault()?.ItemModifierGroups;
    }
}
