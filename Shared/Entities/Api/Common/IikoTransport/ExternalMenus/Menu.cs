using Newtonsoft.Json;

namespace WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.ExternalMenus
{
    /// <summary>
    /// Contains information about external menu by ID.
    /// </summary>
    [JsonObject]
    public class Menu
    {
        /// <summary>
        /// ID of the external menu.
        /// </summary>
        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public int Id { get; set; }

        /// <summary>
        /// External menu name.
        /// </summary>
        [JsonProperty(PropertyName = "name", Required = Required.AllowNull)]
        public string? Name { get; set; }

        /// <summary>
        /// External menu description.
        /// </summary>
        [JsonProperty(PropertyName = "description", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Description { get; set; }

        /// <summary>
        /// External menu categories.
        /// </summary>
        [JsonProperty(PropertyName = "itemCategories", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<MenuCategory>? Categories { get; set; }
    }
}
