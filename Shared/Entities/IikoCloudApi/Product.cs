using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WebAppAssembly.Shared.Entities.IikoCloudApi
{
    public class Product
    {
        /// <summary>
        /// Id of product
        /// </summary>
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        /// <summary>
        /// Code of product. Can be null
        /// </summary>
        [JsonProperty("code")]
        [JsonPropertyName("code")]
        public string? Code { get; set; }
        /// <summary>
        /// Sizes available for that product
        /// </summary>
        [JsonProperty("size")]
        [JsonPropertyName("size")]
        public IEnumerable<string>? Size { get; set; }
    }
}
