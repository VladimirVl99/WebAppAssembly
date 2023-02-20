using Newtonsoft.Json;

namespace WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.Loyalties
{
    /// <summary>
    /// Free product.
    /// </summary>
    [JsonObject]
    public class Product
    {
        /// <summary>
        /// Id of product
        /// </summary>
        [JsonProperty(PropertyName = "id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid? Id { get; set; }

        /// <summary>
        /// Code of product. Can be null
        /// </summary>
        [JsonProperty(PropertyName = "code", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Code { get; set; }

        /// <summary>
        /// Sizes available for that product
        /// </summary>
        [JsonProperty(PropertyName = "size", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<string>? Size { get; set; }
    }
}
