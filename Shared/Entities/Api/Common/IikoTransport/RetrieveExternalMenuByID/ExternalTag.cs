using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.RetrieveExternalMenuByID
{
    public class ExternalTag
    {
        [JsonProperty("code")]
        [JsonPropertyName("code")]
        public string? Code { get; set; }
        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }
}
