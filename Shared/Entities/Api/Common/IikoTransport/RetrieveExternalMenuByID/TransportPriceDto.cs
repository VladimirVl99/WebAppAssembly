using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.RetrieveExternalMenuByID
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
