using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WebAppAssembly.Shared.Entities.Telegram;
using WebAppAssembly.Shared.Models.Order;

namespace WebAppAssembly.Shared.Entities
{
    public class FoodShopInfo
    {
        public FoodShopInfo() { }

        public FoodShopInfo(IEnumerable<DeliveryTerminal>? deliveryTerminals, bool? isTestMode, WebAppInfo? webAppInfo)
        {
            DeliveryTerminals = deliveryTerminals;
            IsTestMode = isTestMode;
            WebAppInfo = webAppInfo;
        }

        [JsonProperty("deliveryTerminals")]
        [JsonPropertyName("deliveryTerminals")]
        public IEnumerable<DeliveryTerminal>? DeliveryTerminals { get; set; }
        [JsonProperty("isTestMode")]
        [JsonPropertyName("isTestMode")]
        public bool? IsTestMode { get; set; }
        [JsonProperty("webAppInfo")]
        [JsonPropertyName("webAppInfo")]
        public WebAppInfo? WebAppInfo { get; set; }
    }
}
