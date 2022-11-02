using Newtonsoft.Json;

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
        public string? City { get; set; }
        [JsonProperty("street")]
        public string? Street { get; set; }
        [JsonProperty("house")]
        public string? House { get; set; }
        [JsonProperty("flat")]
        public string? Flat { get; set; }
        [JsonProperty("entrance")]
        public string? Entrance { get; set; }
        [JsonProperty("floor")]
        public string? Floor { get; set; }
    }
}