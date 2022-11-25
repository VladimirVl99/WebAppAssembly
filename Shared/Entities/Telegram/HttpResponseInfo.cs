using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebAppAssembly.Shared.Entities.Telegram
{
    public class HttpResponseInfo
    {
        [JsonProperty("httpStatusCode")]
        [JsonPropertyName("httpStatusCode")]
        public HttpStatusCode HttpStatusCode { get; set; }
        [JsonProperty("message")]
        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }
}
