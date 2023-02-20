using Newtonsoft.Json;
using WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.Loyalties;

namespace WebAppAssembly.Shared.Entities.Api.Common.Loylties
{
    /// <summary>
    /// Discounts and other loyalty items for an order.
    /// Source: https://api-ru.iiko.services/#tag/Discounts-and-promotions/paths/~1api~11~1loyalty~1iiko~1calculate/post.
    /// </summary>
    public class Checkin
    {
        /// <summary>
        /// Loyalty program results.
        /// </summary>
        [JsonProperty("loyaltyProgramResults")]
        public IEnumerable<LoyaltyProgramResult>? LoyaltyProgramResults { get; set; }

        /// <summary>
        /// Marketing campaigns with available payments.
        /// </summary>
        [JsonProperty("availablePayments")]
        public IEnumerable<AvailablePayment>? AvailablePayments { get; set; }

        /// <summary>
        /// Description of an error.
        /// </summary>
        [JsonProperty("warningMessage")]
        public string? WarningMessage { get; set; }
    }
}
