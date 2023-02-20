using Newtonsoft.Json;
using System.Net;
using SourceHttpResInfo = WebAppAssembly.Shared.Entities.Api.Common.HttpInfos.HttpResponseShortInfo;

namespace WebAppAssembly.Shared.Entities.Api.Requests.HttpInfos
{
    /// <summary>
    /// Information about http response.
    /// </summary>
    [JsonObject]
    public class HttpResponseShortInfo : SourceHttpResInfo
    {
        public HttpResponseShortInfo(HttpStatusCode httpStatusCode, string? message = null)
        {
            HttpStatusCode = httpStatusCode;
            Message = message;
        }
    }
}
