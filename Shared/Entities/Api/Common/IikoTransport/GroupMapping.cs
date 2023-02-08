using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WebAppAssembly.Shared.Entities.Api.Common.IikoTransport
{
    public class GroupMapping
    {
        /// <summary>
        /// Id of combo group
        /// </summary>
        [JsonProperty("groupId")]
        [JsonPropertyName("groupId")]
        public Guid GroupId { get; set; }
        /// <summary>
        /// Id of item, suitable for group
        /// </summary>
        [JsonProperty("itemId")]
        [JsonPropertyName("itemId")]
        public Guid ItemId { get; set; }
    }
}
