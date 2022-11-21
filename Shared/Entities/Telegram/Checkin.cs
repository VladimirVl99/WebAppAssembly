using Newtonsoft.Json;
using System.Text.Json.Serialization;
using WebAppAssembly.Shared.Entities.IikoCloudApi;

namespace WebAppAssembly.Shared.Entities.Telegram
{
    public class Checkin
    {
        [JsonProperty("loyaltyProgramResults")]
        [JsonPropertyName("loyaltyProgramResults")]
        public IEnumerable<LoyaltyProgramResult>? LoyaltyProgramResults { get; set; }
        [JsonProperty("availablePayments")]
        [JsonPropertyName("availablePayments")]
        public IEnumerable<AvailablePayment>? AvailablePayments { get; set; }
        [JsonProperty("warningMessage")]
        [JsonPropertyName("warningMessage")]
        public string? WarningMessage { get; set; }
    }
}
