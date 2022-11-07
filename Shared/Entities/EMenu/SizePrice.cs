using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebAppAssembly.Shared.Entities.EMenu
{
    public class SizePrice
    {
        [JsonProperty("sizeId")]
        [JsonPropertyName("sizeId")]
        public Guid? SizeId { get; set; }
        [Required]
        [JsonProperty("price")]
        [JsonPropertyName("price")]
        public Price? Price { get; set; }
    }
}
