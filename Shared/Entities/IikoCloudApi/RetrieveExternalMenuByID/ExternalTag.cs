using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace ApiServerForTelegram.Entities.IikoCloudApi.General.Menu.RetrieveExternalMenuByID
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
