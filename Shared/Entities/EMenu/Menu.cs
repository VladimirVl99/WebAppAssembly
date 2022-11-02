using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace WebAppAssembly.Shared.Entities.EMenu
{
    public class Menu
    {
        public Guid OrganizationId { get; set; }
        [JsonRequired]
        [JsonProperty("correlationId")]
        public Guid CorrelationId { get; set; }
        [JsonRequired]
        [JsonProperty("groups")]
        public IEnumerable<Group>? Groups { get; set; }
        [JsonRequired]
        [JsonProperty("productCategories")]
        public IEnumerable<ProductCategory>? ProductCategories { get; set; }
        [JsonRequired]
        [JsonProperty("products")]
        public IEnumerable<Product>? Products { get; set; }
        [JsonRequired]
        [JsonProperty("sizes")]
        public IEnumerable<Size>? Sizes { get; set; }
        [JsonRequired]
        [JsonProperty("revision")]
        public long Revision { get; set; }
    }
}
