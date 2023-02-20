using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WebAppAssembly.Shared.Entities.Api.Common.Delivery;
using WebAppAssembly.Shared.Entities.Api.Common.Delivery.Addresses;
using WebAppAssembly.Shared.Entities.Api.Common.General.Terminals;

namespace WebAppAssembly.Shared.Entities.Api.Common.PersonalData
{
    /// <summary>
    /// Contains information about delivery data.
    /// </summary>
    [JsonObject]
    public class PersonalItem : IPersonalItem
    {
        [JsonProperty("deliveryServiceType", Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public DeliveryServiceType DeliveryServiceType { get; set; }

        [JsonProperty("terminalId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid? TerminalId { get; set; }

        [JsonProperty("deliveryTerminal", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DeliveryTerminal? DeliveryTerminal { get; set; }

        [JsonProperty("address", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Address? Address { get; set; }
    }
}
