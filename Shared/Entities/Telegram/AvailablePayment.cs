using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WebAppAssembly.Shared.Entities.Telegram
{
    public class AvailablePayment
    {
        /// <summary>
        /// Marketing campaign id
        /// </summary>
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        /// <summary>
        /// Max sum
        /// </summary>
        [JsonProperty("maxSum")]
        [JsonPropertyName("maxSum")]
        public double MaxSum { get; set; }
        /// <summary>
        /// Payment order. In case of partial payment, payments with lesser order should be filled first
        /// </summary>
        [JsonProperty("order")]
        [JsonPropertyName("order")]
        public int Order { get; set; }
    }
}