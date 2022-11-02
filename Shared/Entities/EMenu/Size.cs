using Newtonsoft.Json;

namespace WebAppAssembly.Shared.Entities.EMenu
{
    public class Size
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("name")]
        public string? Name { get; set; }
        [JsonProperty("priority")]
        public int? Priority { get; set; }
        [JsonProperty("isDefault")]
        public bool IsDefault { get; set; }
    }
}
