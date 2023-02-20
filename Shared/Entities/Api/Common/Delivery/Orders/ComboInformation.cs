using Newtonsoft.Json;

namespace WebAppAssembly.Shared.Entities.Api.Common.Delivery.Orders
{
    /// <summary>
    /// Information about an order combo.
    /// </summary>
    [JsonObject]
    public class ComboInformation
    {
        /// <summary>
        /// Created combo ID
        /// </summary>
        [JsonProperty(PropertyName = "comboId", Required = Required.Always)]
        public Guid ComboId { get; set; }

        /// <summary>
        /// Action ID that defines combo
        /// </summary>
        [JsonProperty(PropertyName = "comboSourceId", Required = Required.Always)]
        public Guid ComboSourceId { get; set; }

        /// <summary>
        /// Combo group ID to which item belongs
        /// </summary>
        [JsonProperty(PropertyName = "comboGroupId", Required = Required.Always)]
        public Guid ComboGroupId { get; set; }
    }
}
