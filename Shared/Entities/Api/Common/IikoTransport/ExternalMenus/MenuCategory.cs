using Newtonsoft.Json;

namespace WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.ExternalMenus
{
    /// <summary>
    /// Contains information about external menu category.
    /// </summary>
    [JsonObject]
    public class MenuCategory
    {
        /// <summary>
        /// External products.
        /// </summary>
        [JsonProperty(PropertyName = "items", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<Product>? Items { get; set; }

        /// <summary>
        /// ID of the category of the external menu.
        /// </summary>
        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public Guid Id { get; set; }

        /// <summary>
        /// Category name of the external menu.
        /// </summary>
        [JsonProperty(PropertyName = "name", Required = Required.AllowNull)]
        public string? Name { get; set; }

        /// <summary>
        /// Category description.
        /// </summary>
        [JsonProperty(PropertyName = "description", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Description { get; set; }

        /// <summary>
        /// Link to image.
        /// </summary>
        [JsonProperty(PropertyName = "buttonImageUrl", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? ButtonImageUrl { get; set; }

        /// <summary>
        /// Link to header image.
        /// </summary>
        [JsonProperty(PropertyName = "headerImageUrl", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? HeaderImageUrl { get; set; }
    }
}
