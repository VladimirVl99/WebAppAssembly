using Newtonsoft.Json;

namespace WebAppAssembly.Shared.Entities.Api.Common.Delivery.Orders
{
    /// <summary>
    /// Information about an item's modifier.
    /// </summary>
    [JsonObject]
    public class SimpleModifier
    {
        /// <summary>
        /// ID.
        /// </summary>
        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public Guid Id { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        public string Name { get; set; } = default!;

        /// <summary>
        /// Minimum amount.
        /// </summary>
        [JsonProperty(PropertyName = "minAmount", Required = Required.Always)]
        public double MinQuantity { get; set; }

        /// <summary>
        /// Maximum amount.
        /// </summary>
        [JsonProperty(PropertyName = "maxAmount", Required = Required.Always)]
        public double MaxQuantity { get; set; }
    }
}
