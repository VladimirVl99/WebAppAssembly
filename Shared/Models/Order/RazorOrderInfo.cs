using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WebAppAssembly.Shared.Models.Order
{
    // Flags for razor pages which is using Telegram app
    public class RazorOrderInfo
    {
        [JsonProperty("pageType")]
        [JsonPropertyName("pageType")]
        public PageType PageType { get; set; }
        [JsonProperty("orderStatusType")]
        [JsonPropertyName("orderStatusType")]
        public OrderStatusType OrderStatusType { get; set; }
        [JsonProperty("isTelegramConnectionInit")]
        [JsonPropertyName("isTelegramConnectionInit")]
        public bool IsTelegramConnectionInit { get; set; } = false;
    }
}