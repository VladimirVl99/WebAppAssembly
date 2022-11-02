using Newtonsoft.Json;

namespace WebAppAssembly.Shared.Entities.CreateDelivery
{
    public class ComboInformation
    {
        // Created combo ID
        [JsonRequired]
        [JsonProperty("comboId")]
        public Guid ComboId { get; set; }
        // Action ID that defines combo
        [JsonRequired]
        [JsonProperty("comboSourceId")]
        public Guid ComboSourceId { get; set; }
        // Combo group ID to which item belongs
        [JsonRequired]
        [JsonProperty("comboGroupId")]
        public Guid ComboGroupId { get; set; }
    }
}
