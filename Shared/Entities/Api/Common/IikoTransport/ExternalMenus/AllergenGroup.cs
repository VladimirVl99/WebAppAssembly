using Newtonsoft.Json;

namespace WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.ExternalMenus
{
    /// <summary>
    /// Contains information about allergen group.
    /// </summary>
    [JsonObject]
    public class AllergenGroup
    {
        /// <summary>
        /// ID.
        /// </summary>
        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public Guid Id { get; set; }

        /// <summary>
        /// Allergen's code.
        /// </summary>
        [JsonProperty(PropertyName = "code", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Code { get; set; }

        /// <summary>
        /// Allergen's name.
        /// </summary>
        [JsonProperty(PropertyName = "name", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Name { get; set; }
    }
}
