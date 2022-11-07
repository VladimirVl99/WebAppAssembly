using Newtonsoft.Json;
using System.Text.Json.Serialization;
using WebAppAssembly.Shared.Models.Order;

namespace WebAppAssembly.Shared.Entities.Telegram
{
    public class WebAppInfo
    {
        [JsonProperty("webAppMenu")]
        [JsonPropertyName("webAppMenu")]
        public WebAppMenu? WebAppMenu { get; set; }
        [JsonProperty("deliveryTerminals")]
        [JsonPropertyName("deliveryTerminals")]
        public IEnumerable<DeliveryTerminal>? DeliveryTerminals { get; set; }
        [JsonProperty("pickupType")]
        [JsonPropertyName("pickupType")]
        public PickupType PickupType { get; set; }
        [JsonProperty("useIikoBizProgram")]
        [JsonPropertyName("useIikoBizProgram")]
        public bool UseIikoBizProgram { get; set; }
        [JsonProperty("useCoupon")]
        [JsonPropertyName("useCoupon")]
        public bool UseCoupon { get; set; }
        [JsonProperty("useDiscountBalance")]
        [JsonPropertyName("useDiscountBalance")]
        public bool UseDiscountBalance { get; set; }
    }
}