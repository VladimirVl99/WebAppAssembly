using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebAppAssembly.Shared.Entities.Telegram
{
    public class LoyaltyCheckinInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public LoyaltyCheckinInfo() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkin"></param>
        /// <param name="ok"></param>
        /// <param name="httpResponseInfo"></param>
        public LoyaltyCheckinInfo(Checkin? checkin, bool ok, HttpResponseInfo? httpResponseInfo)
        {
            Checkin = checkin;
            Ok = ok;
            HttpResponseInfo = httpResponseInfo;
        }

        [JsonProperty("checkin")]
        [JsonPropertyName("checkin")]
        public Checkin? Checkin { get; set; }
        [JsonProperty("ok")]
        [JsonPropertyName("ok")]
        public bool Ok { get; set; }
        [JsonProperty("httpResponseInfo")]
        [JsonPropertyName("httpResponseInfo")]
        public HttpResponseInfo? HttpResponseInfo { get; set; }
    }
}
