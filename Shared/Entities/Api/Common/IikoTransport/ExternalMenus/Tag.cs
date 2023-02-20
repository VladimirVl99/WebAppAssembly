using Newtonsoft.Json;

namespace WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.ExternalMenus
{
    /// <summary>
    /// Contains information about tag.
    /// </summary>
    [JsonObject]
    public class Tag
    {
        [JsonProperty(PropertyName = "name", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Name { get; set; }
    }
}
