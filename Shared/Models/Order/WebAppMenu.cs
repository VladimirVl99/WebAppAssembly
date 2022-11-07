using Newtonsoft.Json;
using System.Text.Json.Serialization;
using WebAppAssembly.Shared.Entities.EMenu;

namespace WebAppAssembly.Shared.Models.Order
{
    public class WebAppMenu
    {
        [JsonProperty("menu")]
        [JsonPropertyName("menu")]
        public Menu? Menu { get; set; }
        [JsonProperty("necessaryProducts")]
        [JsonPropertyName("necessaryProducts")]
        public IEnumerable<Product>? NecessaryProducts { get; set; }
        [JsonProperty("necessaryGroups")]
        [JsonPropertyName("necessaryGroups")]
        public IEnumerable<Group>? NecessaryGroups { get; set; }
        [JsonProperty("groupsWithProducts")]
        [JsonPropertyName("groupsWithProducts")]
        public IEnumerable<GroupWithProducts>? GroupsWithProducts { get; set; }
    }
}