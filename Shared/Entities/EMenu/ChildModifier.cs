using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebAppAssembly.Shared.Entities.EMenu
{
    public class ChildModifier
    {
        [Required]
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonProperty("defaultAmount")]
        [JsonPropertyName("defaultAmount")]
        public int? DefaultAmount { get; set; }
        [Required]
        [JsonProperty("minAmount")]
        [JsonPropertyName("minAmount")]
        public int MinAmount { get; set; }
        [Required]
        [JsonProperty("maxAmount")]
        [JsonPropertyName("maxAmount")]
        public int MaxAmount { get; set; }
        [JsonProperty("required")]
        [JsonPropertyName("required")]
        public bool? Required { get; set; }
        [JsonProperty("hideIfDefaultAmount")]
        [JsonPropertyName("hideIfDefaultAmount")]
        public bool? HideIfDefaultAmount { get; set; }
        [JsonProperty("splittable")]
        [JsonPropertyName("splittable")]
        public bool? Splittable { get; set; }
        [JsonProperty("freeOfChargeAmount")]
        [JsonPropertyName("freeOfChargeAmount")]
        public int? FreeOfChargeAmount { get; set; }
    }
}
