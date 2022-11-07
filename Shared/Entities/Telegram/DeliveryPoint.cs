using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WebAppAssembly.Shared.Entities.Telegram
{
    public class DeliveryPoint
    {
        public DeliveryPoint() { }
        public DeliveryPoint(string city, string street, string house, string? flat = null, string? entrance = null, string? floor = null)
        {
            City = city;
            Street = street;
            House = house;
            Flat = flat;
            Entrance = entrance;
            Floor = floor;
        }

        [JsonProperty("city")]
        [JsonPropertyName("city")]
        public string? City { get; set; }
        [JsonProperty("street")]
        [JsonPropertyName("street")]
        public string? Street { get; set; }
        [JsonProperty("house")]
        [JsonPropertyName("house")]
        public string? House { get; set; }
        [JsonProperty("flat")]
        [JsonPropertyName("flat")]
        public string? Flat { get; set; }
        [JsonProperty("entrance")]
        [JsonPropertyName("entrance")]
        public string? Entrance { get; set; }
        [JsonProperty("floor")]
        [JsonPropertyName("floor")]
        public string? Floor { get; set; }
    }
}