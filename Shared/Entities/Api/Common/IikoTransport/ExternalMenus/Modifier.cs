using Newtonsoft.Json;

namespace WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.ExternalMenus
{
    /// <summary>
    /// Contains information about a modifier.
    /// </summary>
    [JsonObject]
    public class Modifier
    {
        /// <summary>
        /// Modifier's Id.
        /// </summary>
        [JsonProperty(PropertyName = "itemId", Required = Required.Always)]
        public Guid Id { get; set; }

        /// <summary>
        /// Modifier's prices.
        /// </summary>
        [JsonProperty(PropertyName = "prices", Required = Required.Always)]
        public IEnumerable<Price> Prices { get; set; } = default!;

        /// <summary>
        /// Modifier's code.
        /// </summary>
        [JsonProperty(PropertyName = "sku", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Code { get; set; }

        /// <summary>
        /// Modifier's name.
        /// </summary>
        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        public string Name { get; set; } = default!;

        /// <summary>
        /// Modifier's description.
        /// </summary>
        [JsonProperty(PropertyName = "description", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Description { get; set; }

        /// <summary>
        /// Links to images.
        /// </summary>
        [JsonProperty(PropertyName = "buttonImage", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? ButtonImage { get; set; }

        /// <summary>
        /// Restrictions.
        /// </summary>
        [JsonProperty(PropertyName = "restrictions", Required = Required.Always)]
        public Restriction Restrictions { get; set; } = default!;

        /// <summary>
        /// Allergen groups.
        /// </summary>
        [JsonProperty(PropertyName = "allergenGroups", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<AllergenGroup>? AllergenGroups { get; set; }

        /// <summary>
        /// Nutrition per hundred grams.
        /// </summary>
        [JsonProperty(PropertyName = "nutritionPerHundredGrams", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Nutrition? NutritionPerHundredGrams { get; set; }

        /// <summary>
        /// Nutritions.
        /// </summary>
        [JsonProperty(PropertyName = "nutritions", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<Nutrition>? Nutritions { get; set; }

        /// <summary>
        /// Modifier's weight in gramms.
        /// </summary>
        [JsonProperty(PropertyName = "portionWeightGrams", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public float? PortionWeightGrams { get; set; }

        /// <summary>
        /// Internal tags.
        /// </summary>
        [JsonProperty(PropertyName = "tags", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<InternalTag>? InternalTags { get; set; }

        /// <summary>
        /// External tags.
        /// </summary>
        [JsonProperty(PropertyName = "labeles", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<ExternalTag>? ExternalTags { get; set; }


        public double Price() => Prices.FirstOrDefault()?.Amount ?? 0;
    }
}
