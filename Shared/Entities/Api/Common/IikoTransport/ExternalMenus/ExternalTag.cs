using Newtonsoft.Json;

namespace WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.ExternalMenus
{
    /// <summary>
    /// Contains information about an external tag.
    /// </summary>
    [JsonObject]
    public class ExternalTag : Tag
    {
        /// <summary>
        /// Tag code.
        /// </summary>
        [JsonProperty(PropertyName = "code", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Code { get; set; }
    }
}
