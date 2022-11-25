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
