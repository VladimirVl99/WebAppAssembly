using ApiServerForTelegram.Entities.EExceptions;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using WebAppAssembly.Shared.Entities.EMenu;
using WebAppAssembly.Shared.Models.Order;

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

        public bool HaveModifiersOrSeveralSizes() => HaveModifiers() || HaveSizesMoreThanOne();

        public float? PriceOrDefault(Guid? sizeId = null) => 
            sizeId is null ? ItemSizes?.FirstOrDefault()?.Prices?.FirstOrDefault()?.Price : ItemSizes?.FirstOrDefault(x => x.SizeId == sizeId)?.Prices?.FirstOrDefault()?.Price;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sizeId"></param>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        public float Price(Guid? sizeId = null) => PriceOrDefault(sizeId) ?? throw new InfoException(typeof(TransportItemDto).FullName!,
            nameof(Price), nameof(Exception), $"Price of size ID - '{(sizeId is null ? "default ID" : sizeId)}' is null");
        private string IntOrTwoNumberOfDigitsFromCurrentCulture(float number)
            => ((int)(number * 100) % 100) != 0 ? string.Format("{0:F2}", number) : ((int)number).ToString();
        public float Weight() => ItemSizes?.FirstOrDefault()?.PortionWeightGrams ?? 0;
        public string WeightAsString() => IntOrTwoNumberOfDigitsFromCurrentCulture(Weight());
        public float Fats() => ItemSizes?.FirstOrDefault()?.NutritionPerHundredGrams?.Fats ?? 0;
        public string FatsAsString() => IntOrTwoNumberOfDigitsFromCurrentCulture(Fats());
        public float Proteins() => ItemSizes?.FirstOrDefault()?.NutritionPerHundredGrams?.Proteins ?? 0;
        public string ProteinsAsString() => IntOrTwoNumberOfDigitsFromCurrentCulture(Proteins());
        public float Carbs() => ItemSizes?.FirstOrDefault()?.NutritionPerHundredGrams?.Carbs ?? 0;
        public string CarbsAsString() => IntOrTwoNumberOfDigitsFromCurrentCulture(Carbs());
        public float Energy() => ItemSizes?.FirstOrDefault()?.NutritionPerHundredGrams?.Energy ?? 0;
        public string EnergyAsString() => IntOrTwoNumberOfDigitsFromCurrentCulture(Energy());
        public IEnumerable<TransportModifierGroupDto>? ModifierGroups() => ItemSizes?.FirstOrDefault()?.ItemModifierGroups;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        public Guid GetItemId() => ItemId ?? throw new InfoException(typeof(TransportItemDto).FullName!,
            nameof(GetItemId), nameof(Exception), $"Item ID can't be null");
    }
}
