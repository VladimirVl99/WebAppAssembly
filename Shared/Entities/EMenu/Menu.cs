using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebAppAssembly.Shared.Entities.EMenu
{
    public class Menu
    {
        [JsonProperty("organizationId")]
        [JsonPropertyName("organizationId")]
        public Guid OrganizationId { get; set; }
        [JsonRequired]
        [JsonProperty("correlationId")]
        [JsonPropertyName("correlationId")]
        public Guid CorrelationId { get; set; }
        [JsonRequired]
        [JsonProperty("groups")]
        [JsonPropertyName("groups")]
        public IEnumerable<Group>? Groups { get; set; }
        [JsonRequired]
        [JsonProperty("productCategories")]
        [JsonPropertyName("productCategories")]
        public IEnumerable<ProductCategory>? ProductCategories { get; set; }
        [JsonRequired]
        [JsonProperty("products")]
        [JsonPropertyName("products")]
        public IEnumerable<Product>? Products { get; set; }
        [JsonRequired]
        [JsonProperty("sizes")]
        [JsonPropertyName("sizes")]
        public IEnumerable<Size>? Sizes { get; set; }
        [JsonRequired]
        [JsonProperty("revision")]
        [JsonPropertyName("revision")]
        public long Revision { get; set; }
    }
}
