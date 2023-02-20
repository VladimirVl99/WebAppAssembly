using Newtonsoft.Json;

namespace WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.ExternalMenus
{
    /// <summary>
    /// Contains information about a product's nutrition per hundred grams.
    /// </summary>
    [JsonObject]
    public class Nutrition
    {
        /// <summary>
        /// Fat per 100g.
        /// </summary>
        [JsonProperty(PropertyName = "fats", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public float? Fats { get; set; }

        /// <summary>
        /// Protein per 100g.
        /// </summary>
        [JsonProperty(PropertyName = "proteins", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public float? Proteins { get; set; }

        /// <summary>
        /// Carbohydrate per 100g.
        /// </summary>
        [JsonProperty(PropertyName = "carbs", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public float? Carbs { get; set; }

        /// <summary>
        /// Calories per 100g.
        /// </summary>
        [JsonProperty(PropertyName = "energy", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public float? Energy { get; set; }

        /// <summary>
        /// List of organizations.
        /// </summary>
        [JsonProperty(PropertyName = "organizations", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<Guid>? OrganizationIds { get; set; }
    }
}
