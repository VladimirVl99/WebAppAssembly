using Newtonsoft.Json;

namespace WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.ExternalMenus
{
    /// <summary>
    /// Contains information about a product size.
    /// </summary>
    [JsonObject]
    public class Size
    {
        /// <summary>
        /// Size's prices.
        /// </summary>
        [JsonProperty(PropertyName = "prices", Required = Required.Always)]
        public IEnumerable<Price> Prices { get; set; } = default!;

        /// <summary>
        /// Size's group modifiers.
        /// </summary>
        [JsonProperty(PropertyName = "itemModifierGroups", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<GroupModifier>? ModifierGroups { get; set; }

        /// <summary>
        /// Unique size code, consists of the product code and the name of the size,
        /// if the product has one size, then the size code will be equal to the product code.
        /// </summary>
        [JsonProperty(PropertyName = "sku", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Code { get; set; }

        /// <summary>
        /// Product size's code.
        /// </summary>
        [JsonProperty(PropertyName = "sizeCode", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? SizeCode { get; set; }

        /// <summary>
        /// Name of the product size, the name can be empty if there is only one size in the list.
        /// </summary>
        [JsonProperty(PropertyName = "sizeName", Required = Required.AllowNull)]
        public string? SizeName { get; set; }

        /// <summary>
        /// Whether it is a default size of the product. If the product has one size,
        /// then the parameter will be true, if the product has several sizes, none of them can be default.
        /// </summary>
        [JsonProperty(PropertyName = "isDefault", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? IsDefault { get; set; }

        /// <summary>
        /// Size's weight.
        /// </summary>
        [JsonProperty(PropertyName = "portionWeightGrams", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public float? PortionWeightGrams { get; set; }

        /// <summary>
        /// ID size, can be empty if the default size is selected and it is the only size in the list.
        /// </summary>
        [JsonProperty(PropertyName = "sizeId", Required = Required.Always)]
        public Guid SizeId { get; set; }

        /// <summary>
        /// Nutrition per hundred grams.
        /// </summary>
        [JsonProperty(PropertyName = "nutritionPerHundredGrams", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Nutrition? NutritionPerHundredGrams { get; set; }

        /// <summary>
        /// Size's nutritions.
        /// </summary>
        [JsonProperty(PropertyName = "nutritions", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<Nutrition>? Nutritions { get; set; }

        /// <summary>
        /// Link to image.
        /// </summary>
        [JsonProperty(PropertyName = "buttonImageUrl", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? ButtonImageUrl { get; set; }

        /// <summary>
        /// Cropped url link to image.
        /// </summary>
        [JsonProperty(PropertyName = "buttonImageCroppedUrl", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<string>? ButtonImageCroppedUrl { get; set; }
    }
}
