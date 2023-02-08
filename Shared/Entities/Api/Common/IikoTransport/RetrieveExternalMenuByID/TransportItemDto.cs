using ApiServerForTelegram.Entities.EExceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using WebAppAssembly.Shared.Entities.Api.Common.Delivery.Orders;
using WebAppAssembly.Shared.Entities.EMenu;

namespace WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.RetrieveExternalMenuByID
{
    public class TransportItemDto
    {
        [JsonProperty("itemSizes")]
        [JsonPropertyName("itemSizes")]
        public IEnumerable<TransportItemSizeDto>? ItemSizes { get; set; }
        [JsonProperty("sku")]
        [JsonPropertyName("sku")]
        public string? Sku { get; set; }

        private string? _name;

        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string Name
        {
            get => _name ?? "???";
            set => _name = value;
        }
        [JsonProperty("description")]
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonProperty("allergenGroups")]
        [JsonPropertyName("allergenGroups")]
        public IEnumerable<AllergenGroupDto>? AllergenGroups { get; set; }

        private Guid _itemId;

        [JsonProperty("itemId")]
        [JsonPropertyName("itemId")]
        public Guid ItemId
        {
            get
            {
                return _itemId;
            }
            set
            {
                var res = IsItemIdCorrect(value);
                if (!res.Item1)
                    throw new InfoException(typeof(TransportItemDto).FullName!, nameof(Exception), res.Item2);
                _itemId = value;
            }
        }
        [JsonProperty("modifierSchemaId")]
        [JsonPropertyName("modifierSchemaId")]
        public Guid? ModifierSchemaId { get; set; }
        [JsonProperty("taxCategory")]
        [JsonPropertyName("taxCategory")]
        public IEnumerable<TaxCategoryDto>? TaxCategories { get; set; }
        [JsonProperty("orderItemType")]
        [JsonPropertyName("orderItemType")]
        [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
        public OrderItemType OrderItemType { get; set; }
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

        public float? PriceOrNull(Guid? sizeId = null) =>
            sizeId is null ? ItemSizes?.FirstOrDefault()?.Prices?.FirstOrDefault()?.Price : ItemSizes?.FirstOrDefault(x => x.SizeId == sizeId)?.Prices?.FirstOrDefault()?.Price;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sizeId"></param>
        /// <returns></returns>
        /// <exception cref="InfoException"></exception>
        public float Price(Guid? sizeId = null) => PriceOrNull(sizeId) ?? throw new InfoException(typeof(TransportItemDto).FullName!,
            nameof(Price), nameof(Exception), $"Price of size ID - '{(sizeId is null ? "default ID" : sizeId)}' is null");
        private string IntOrTwoNumberOfDigitsFromCurrentCulture(float number)
            => (int)(number * 100) % 100 != 0 ? string.Format("{0:F2}", number) : ((int)number).ToString();
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
        /// <param name="id"></param>
        /// <returns></returns>
        private static (bool, string) IsItemIdCorrect(Guid? id) => id switch
        {
            null => (false, $"{nameof(ItemId)} cannot be null."),
            Guid when id == Guid.Empty => (false, $"{nameof(ItemId)} cannot be equal to {Guid.Empty}."),
            _ => (true, string.Empty)
        };
    }
}
