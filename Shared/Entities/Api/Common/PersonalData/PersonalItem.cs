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
        public DeliveryServiceType DeliveryServiceType { get; set; }
      
        public Guid? TerminalId { get; set; }

        public DeliveryTerminal? DeliveryTerminal { get; set; }

        public Address? Address { get; set; }
    }
}
