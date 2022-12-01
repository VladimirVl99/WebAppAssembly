using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WebAppAssembly.Shared.Entities.Telegram;
using WebAppAssembly.Shared.Models.Order;

namespace WebAppAssembly.Shared.Entities.OfServerSide
{
    public class MainInfoForWebAppOrder
    {
        public MainInfoForWebAppOrder(OrderModelOfServer orderInfo, DeliveryGeneralInfo deliveryGeneralInfo, bool isReleaseMode)
        {
            OrderInfo = orderInfo;
            DeliveryGeneralInfo = deliveryGeneralInfo;
            IsReleaseMode = isReleaseMode;
        }


        [JsonProperty("orderInfo")]
        [JsonPropertyName("orderInfo")]
        public OrderModelOfServer OrderInfo { get; set; }
        [JsonProperty("deliveryGeneralInfo")]
        [JsonPropertyName("deliveryGeneralInfo")]
        public DeliveryGeneralInfo DeliveryGeneralInfo { get; set; }
        [JsonProperty("isReleaseMode")]
        [JsonPropertyName("isReleaseMode")]
        public bool IsReleaseMode { get; set; }
    }
}
