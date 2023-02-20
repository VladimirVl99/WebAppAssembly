using Newtonsoft.Json;
using WebAppAssembly.Shared.Entities.Api.Common.HttpInfos;

namespace WebAppAssembly.Shared.Entities.Api.Common.Loylties
{
    /// <summary>
    /// Information about the loyalty checkin calculation.
    /// </summary>
    public class LoyaltyCheckinInfo
    {
        /// <summary>
        /// Discounts and other loyalty items for an order.
        /// </summary>
        [JsonProperty(PropertyName = "checkin", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Checkin? Checkin { get; set; }

        /// <summary>
        /// Status of checkin calculation.
        /// </summary>
        [JsonProperty(PropertyName = "ok", Required = Required.Always)]
        public bool Ok { get; set; }

        /// <summary>
        /// Response result.
        /// </summary>
        [JsonProperty(PropertyName = "httpResponseInfo", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public HttpResponseShortInfo? HttpResponseInfo { get; set; }

        /// <summary>
        /// If the 'Ok' property has false then calculation of an order's checkin can be skipped
        /// by the Telegram interactive interface, otherwise this property have to be equals to 'Success'.
        /// </summary>
        [JsonProperty(PropertyName = "loyaltyProgramProcessedStatus", Required = Required.Default,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public LoyaltyProgramProcessedStatus LoyaltyProgramProcessedStatus { get; set; }
    }
}
