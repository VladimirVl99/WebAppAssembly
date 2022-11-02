using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace WebAppAssembly.Shared.Entities.EMenu
{
    public class GroupModifier
    {
        [Required]
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [Required]
        [JsonProperty("minAmount")]
        public int MinAmount { get; set; }
        [Required]
        [JsonProperty("maxAmount")]
        public int MaxAmount { get; set; }
        [Required]
        [JsonProperty("required")]
        public bool Required { get; set; }
        [JsonProperty("childModifiersHaveMinMaxRestrictions")]
        public bool ChildModifiersHaveMinMaxRestrictions { get; set; }
        [Required]
        [JsonProperty("childModifiers")]
        public IEnumerable<ChildModifier>? ChildModifiers { get; set; }
        [JsonProperty("hideIfDefaultAmount")]
        public bool? HideDefaultAmount { get; set; }
        [JsonProperty("defaultAmount")]
        public int? DefaultAmount { get; set; }
        [JsonProperty("splittable")]
        public bool? Splittable { get; set; }
        [JsonProperty("freeOfChargeAmount")]
        public int? FreeOfChargeAmount { get; set; }
    }
}
