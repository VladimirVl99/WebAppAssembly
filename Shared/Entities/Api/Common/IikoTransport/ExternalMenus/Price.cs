using Newtonsoft.Json;

namespace WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.ExternalMenus
{
    /// <summary>
    /// Contains information about a size's price.
    /// </summary>
    [JsonObject]
    public class Price
    {
        /// <summary>
        /// Organization IDs.
        /// </summary>
        [JsonProperty(PropertyName = "organizations", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<Guid>? OrganizationIds { get; set; }

        /// <summary>
        /// Product size prices for the organization, if the value is null, then the product/size is not for sale,
        /// the price always belongs to the price category that was selected at the time of the request.
        /// </summary>
        [JsonProperty(PropertyName = "price", Required = Required.Always)]
        public float Amount { get; set; }
    }
}
