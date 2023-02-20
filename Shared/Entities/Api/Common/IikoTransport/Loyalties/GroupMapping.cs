using Newtonsoft.Json;

namespace WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.Loyalties
{
    /// <summary>
    /// Group contained in combo.
    /// </summary>
    [JsonObject]
    public class GroupMapping
    {
        /// <summary>
        /// Id of combo group
        /// </summary>
        [JsonProperty(PropertyName = "groupId", Required = Required.Always)]
        public Guid GroupId { get; set; }

        /// <summary>
        /// Id of item, suitable for group
        /// </summary>
        [JsonProperty(PropertyName = "itemId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid? ItemId { get; set; }
    }
}
