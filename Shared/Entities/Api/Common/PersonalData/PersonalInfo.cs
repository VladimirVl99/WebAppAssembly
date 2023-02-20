using Newtonsoft.Json;
using WebAppAssembly.Shared.Entities.Api.Common.Delivery.Orders;

namespace WebAppAssembly.Shared.Entities.Api.Common.PersonalData
{
    /// <summary>
    /// Contains information about personal info by orders.
    /// </summary>
    [JsonObject]
    public class PersonalInfo
    {
        /// <summary>
        /// Order information.
        /// </summary>
        [JsonProperty(PropertyName = "order", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Order? Order { get; set; }

        /// <summary>
        /// Telegram's chat information.
        /// </summary>
        [JsonProperty(PropertyName = "chat", Required = Required.Always)]
        public TgChat ChatInfo { get; set; } = default!;

        /// <summary>
        /// User's personal data that is stored on the server and can be reused.
        /// </summary>
        [JsonProperty(PropertyName = "item", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public PersonalItem? PersonalItem { get; set; }
    }
}
