using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using WebAppAssembly.Shared.Entities.Api.Common.Delivery;
using WebAppAssembly.Shared.Entities.Api.Common.Delivery.Addresses;
using WebAppAssembly.Shared.Entities.Api.Common.General.Terminals;

namespace WebAppAssembly.Shared.Entities.Api.Common.PersonalData
{
    /// <summary>
    /// Contains information about delivery data.
    /// </summary>
    public interface IPersonalItem
    {
        /// <summary>
        /// The delivery method.
        /// </summary>
        [JsonProperty("deliveryServiceType", Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        DeliveryServiceType DeliveryServiceType { get; set; }

        /// <summary>
        /// A pickup terminal ID.
        /// </summary>
        [JsonProperty("terminalId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        Guid? TerminalId { get; }

        /// <summary>
        /// Additional information about a pickup terminal ID.
        /// </summary>
        [JsonProperty("deliveryTerminal", DefaultValueHandling = DefaultValueHandling.Ignore)]
        DeliveryTerminal? DeliveryTerminal { get; }

        /// <summary>
        /// A client's address.
        /// </summary>
        [JsonProperty("address", DefaultValueHandling = DefaultValueHandling.Ignore)]
        Address? Address { get; }
    }
}
