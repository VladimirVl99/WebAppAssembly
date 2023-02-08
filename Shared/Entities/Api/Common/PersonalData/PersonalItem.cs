using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppAssembly.Shared.Entities.Api.Common.Delivery;
using WebAppAssembly.Shared.Entities.Api.Common.Delivery.Addresses;
using WebAppAssembly.Shared.Entities.Api.Common.General.Terminals;

namespace WebAppAssembly.Shared.Entities.Api.Common.PersonalData
{
    /// <summary>
    /// Contains information about delivery data.
    /// </summary>
    [JsonObject]
    public class PersonalItem
    {
        /// <summary>
        /// The delivery method.
        /// </summary>
        [JsonProperty("deliveryServiceType", Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public DeliveryServiceType DeliveryServiceType { get; set; }

        /// <summary>
        /// A pickup terminal ID.
        /// </summary>
        [JsonProperty("terminalId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid? TerminalId { get; set; }

        /// <summary>
        /// Additional information about a pickup terminal ID.
        /// </summary>
        [JsonProperty("deliveryTerminal", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DeliveryTerminal? DeliveryTerminal { get; set; }

        /// <summary>
        /// A client's address.
        /// </summary>
        [JsonProperty("address", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Address? Address { get; set; }
    }
}
