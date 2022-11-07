using Newtonsoft.Json;
using System.Text.Json.Serialization;
using WebAppAssembly.Shared.Entities.EMenu;

namespace WebAppAssembly.Shared.Models.Order
{
    public class GroupWithProducts
    {
        [JsonProperty("groupId")]
        [JsonPropertyName("groupId")]
        public Guid GroupId { get; set; }
        [JsonProperty("products")]
        [JsonPropertyName("products")]
        public IEnumerable<Product>? Products { get; set; }
    }
}