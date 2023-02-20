using Newtonsoft.Json;

namespace WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.Loyalties
{
    /// <summary>
    /// Program free products.
    /// </summary>
    [JsonObject]
    public class FreeProduct
    {
        /// <summary>
        /// Id of action that caused the suggestion
        /// </summary>
        [JsonProperty(PropertyName = "sourceActionId", Required = Required.Always)]
        public Guid SourceActionId { get; set; }

        /// <summary>
        /// Description for user. Can be null
        /// </summary>
        [JsonProperty(PropertyName = "descriptionForUser", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? DescriptionForUser { get; set; }

        /// <summary>
        /// Products that should be added to order
        /// </summary>
        [JsonProperty(PropertyName = "products", Required = Required.Always)]
        public IEnumerable<Product> Products { get; set; } = default!;
    }
}
