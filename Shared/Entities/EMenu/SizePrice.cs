using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace WebAppAssembly.Shared.Entities.EMenu
{
    public class SizePrice
    {
        [JsonProperty("sizeId")]
        public Guid? SizeId { get; set; }
        [Required]
        [JsonProperty("price")]
        public Price? Price { get; set; }
    }
}
