using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace WebAppAssembly.Shared.Entities.EMenu
{
    public class ProductCategory
    {
        [Required]
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [Required]
        [JsonProperty("name")]
        public string? Name { get; set; }
        [Required]
        [JsonProperty("isDeleted")]
        public bool IsDeleted { get; set; }
    }
}
