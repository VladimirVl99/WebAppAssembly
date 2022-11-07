using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebAppAssembly.Shared.Entities.EMenu
{
    public class ProductCategory
    {
        [Required]
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [Required]
        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [Required]
        [JsonProperty("isDeleted")]
        [JsonPropertyName("isDeleted")]
        public bool IsDeleted { get; set; }
    }
}
