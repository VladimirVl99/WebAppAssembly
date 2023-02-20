using Newtonsoft.Json;

namespace WebAppAssembly.Shared.Entities.Api.Common.Delivery.Orders
{
    /// <summary>
    /// Information about an order's modifier of item.
    /// </summary>
    [JsonObject]
    public class Modifier
    {
        /// <summary>
        /// Modifier item ID
        /// Can be obtained by /api/1/nomenclature operation.
        /// </summary>
        [JsonProperty(PropertyName = "productId", Required = Required.Always)]
        public Guid ProductId { get; set; }

        /// <summary>
        /// Quantity
        /// </summary>
        [JsonProperty(PropertyName = "amount", Required = Required.Always)]
        public double Amount { get; set; }

        /// <summary>
        /// Modifiers group ID (for group modifier). Required for a group modifier.
        /// Can be obtained by /api/1/nomenclature operation.
        /// </summary>
        [JsonProperty(PropertyName = "productGroupId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid? ProductGroupId { get; set; }

        /// <summary>
        /// Unit price.
        /// </summary>
        [JsonProperty(PropertyName = "price", Required = Required.Always)]
        public double Price { get; set; }

        /// <summary>
        /// Unique identifier of the item in the order. MUST be unique for the whole system.
        /// Therefore it must be generated with Guid.NewGuid().
        /// If sent null, it generates automatically on iikoTransport side.
        /// </summary>
        [JsonProperty(PropertyName = "positionId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid? PositionId { get; set; }

        /// <summary>
        /// Minimum amount.
        /// Can be null???
        /// </summary>
        [JsonProperty(PropertyName = "minAmount", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double? MinAmount { get; set; }

        /// <summary>
        /// Maximum amount.
        /// Can be null???
        /// </summary>
        [JsonProperty(PropertyName = "maxAmount", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double? MaxAmount { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        public string Name { get; set; } = default!;
    }
}