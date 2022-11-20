using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace ApiServerForTelegram.Entities.IikoCloudApi.General.Menu.RetrieveExternalMenuByID
{
    public class TransportPriceDto
    {
        [JsonProperty("organizationId")]
        [JsonPropertyName("organizationId")]
        public Guid? Organizations { get; set; }
        [JsonProperty("price")]
        [JsonPropertyName("price")]
        public float Price { get; set; }
    }
}
