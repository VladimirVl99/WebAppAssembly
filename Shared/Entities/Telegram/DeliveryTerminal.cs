using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WebAppAssembly.Shared.Entities.Telegram
{
    public class DeliveryTerminal
    {
        public DeliveryTerminal() { }

        public DeliveryTerminal(Guid id, string? name)
        {
            Id = id;
            Name = name;
        }

        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }
}
