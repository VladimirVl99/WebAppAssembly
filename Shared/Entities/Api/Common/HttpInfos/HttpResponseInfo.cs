using Newtonsoft.Json;
using System.Net;

namespace WebAppAssembly.Shared.Entities.Api.Common.HttpInfos
{
    /// <summary>
    /// Information about http response.
    /// </summary>
    [JsonObject]
    public class HttpResponseShortInfo
    {
        /// <summary>
        /// Http code.
        /// </summary>
        [JsonProperty(PropertyName = "httpStatusCode", Required = Required.Always)]
        public HttpStatusCode HttpStatusCode { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        [JsonProperty(PropertyName = "message", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Message { get; set; }
    }
}
