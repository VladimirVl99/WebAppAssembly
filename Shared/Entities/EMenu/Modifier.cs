using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace WebAppAssembly.Shared.Entities.EMenu
{
    public class Modifier
    {
        [Required]
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("defaultAmount")]
        public int? DefaultAmount { get; set; }
        [Required]
        [JsonProperty("minAmount")]
        public int MinAmount { get; set; }
        [Required]
        [JsonProperty("maxAmount")]
        public int MaxAmount { get; set; }
        [JsonProperty("required")]
        public bool? Required { get; set; }
        [JsonProperty("hideIfDefaultAmount")]
        public bool? HideIfDefaultAmount { get; set; }
        [JsonProperty("splittable")]
        public bool? Splittable { get; set; }
        [JsonProperty("freeOfChargeAmount")]
        public int? FreeOfChargeAmount { get; set; }
    }
}
