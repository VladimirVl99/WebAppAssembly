using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WebAppAssembly.Shared.Entities.IikoCloudApi
{
    public class FreeProduct
    {
        /// <summary>
        /// Id of action that caused the suggestion
        /// </summary>
        [JsonProperty("sourceActionId")]
        [JsonPropertyName("sourceActionId")]
        public Guid SourceActionId { get; set; }
        /// <summary>
        /// Description for user. Can be null
        /// </summary>
        [JsonProperty("descriptionForUser")]
        [JsonPropertyName("descriptionForUser")]
        public string? DescriptionForUser { get; set; }
        /// <summary>
        /// Products that should be added to order
        /// </summary>
        [JsonProperty("products")]
        [JsonPropertyName("products")]
        public IEnumerable<Product>? Products { get; set; }
    }
}
