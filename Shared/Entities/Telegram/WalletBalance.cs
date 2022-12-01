using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WebAppAssembly.Shared.Entities.Telegram
{
    public class WalletBalance
    {
        [JsonProperty("balance")]
        [JsonPropertyName("balance")]
        public double Balance { get; set; }
    }
}
