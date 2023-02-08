using Newtonsoft.Json;
using System.Text.Json.Serialization;
using WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.RetrieveExternalMenuByID;
using WebAppAssembly.Shared.Entities.EMenu;

namespace WebAppAssembly.Shared.Models.OrderData
{
    public class WebAppMenu
    {
        [JsonProperty("itemCategories")]
        [JsonPropertyName("itemCategories")]
        public IEnumerable<TransportMenuCategoryDto>? ItemCategories { get; set; }
    }
}