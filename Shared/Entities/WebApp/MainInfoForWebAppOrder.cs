﻿using Newtonsoft.Json;
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
        public MainInfoForWebAppOrder(OrderClientModel orderInfo, WebAppInfo deliveryGeneralInfo, bool isReleaseMode, string btnColor)
        {
            OrderInfo = orderInfo;
            DeliveryGeneralInfo = deliveryGeneralInfo;
            IsReleaseMode = isReleaseMode;
            TlgMainBtnColor = btnColor;
        }


        [JsonProperty("orderInfo")]
        [JsonPropertyName("orderInfo")]
        public OrderClientModel OrderInfo { get; set; }
        [JsonProperty("deliveryGeneralInfo")]
        [JsonPropertyName("deliveryGeneralInfo")]
        public WebAppInfo DeliveryGeneralInfo { get; set; }
        [JsonProperty("isReleaseMode")]
        [JsonPropertyName("isReleaseMode")]
        public bool IsReleaseMode { get; set; }
        [JsonProperty("tlgMainBtnColor")]
        [JsonPropertyName("tlgMainBtnColor")]
        public string TlgMainBtnColor { get; set; }
    }
}
