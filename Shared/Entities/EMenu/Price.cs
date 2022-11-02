using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace WebAppAssembly.Shared.Entities.EMenu
{
    public class Price
    {
        [Required]
        [JsonProperty("currentPrice")]
        public double CurrentPrice { get; set; }
        [Required]
        [JsonProperty("isIncludedInMenu")]
        public bool IsIncludedInMenu { get; set; }
        [JsonProperty("nextPrice")]
        public double? NextPrice { get; set; }
        [Required]
        [JsonProperty("nextIncludedInMenu")]
        public bool NextIncludedInMenu { get; set; }
        [JsonProperty("nextDatePrice")]
        public string? NextDatePrice { get; set; }
    }
}
