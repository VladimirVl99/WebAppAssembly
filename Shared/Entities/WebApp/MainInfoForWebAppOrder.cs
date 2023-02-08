using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WebAppAssembly.Shared.Entities.Api.Common.PersonalData;
using WebAppAssembly.Shared.Entities.Telegram;
using WebAppAssembly.Shared.Models.OrderData;

namespace WebAppAssembly.Shared.Entities.WebApp
{
    /// <summary>
    /// 
    /// </summary>
    [JsonObject]
    public class MainInfoForWebAppOrder
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "orderInfo")]
        public PersonalInfo? PersonalInfoOfOrder { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "deliveryGeneralInfo")]
        public GeneralInfoOfOnlineStore? DeliveryGeneralInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "isReleaseMode")]
        public bool IsReleaseMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "tlgMainBtnColor")]
        public string? TlgMainBtnColor { get; set; }
    }
}
