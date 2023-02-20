using Newtonsoft.Json;

namespace WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.ExternalMenus
{
    /// <summary>
    /// Contains information about a group modifier.
    /// </summary>
    [JsonObject]
    public class GroupModifier
    {
        /// <summary>
        /// Modifiers.
        /// </summary>
        [JsonProperty(PropertyName = "responseType", Required = Required.Always)]
        public IEnumerable<Modifier> Items { get; set; } = default!;

        /// <summary>
        /// Modifiers group name.
        /// </summary>
        [JsonProperty(PropertyName = "responseType", Required = Required.AllowNull)]
        public string Name { get; set; } = default!;

        /// <summary>
        /// Modifiers group description.
        /// </summary>
        [JsonProperty(PropertyName = "responseType", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Description { get; set; }

        /// <summary>
        /// Modifier group restrictions.
        /// </summary>
        [JsonProperty(PropertyName = "responseType", Required = Required.Always)]
        public Restriction Restrictions { get; set; } = default!;

        /// <summary>
        /// Whether the modifier can be splitted.
        /// </summary>
        [JsonProperty(PropertyName = "responseType", Required = Required.DisallowNull)]
        public bool CanBeDivided { get; set; }

        /// <summary>
        /// Modifiers group id.
        /// </summary>
        [JsonProperty(PropertyName = "responseType", Required = Required.Always)]
        public Guid ItemGroupId { get; set; }

        /// <summary>
        /// Whether child modifiers can have their own restrictions, or only group ones.
        /// </summary>
        [JsonProperty(PropertyName = "responseType", Required = Required.DisallowNull)]
        public bool ChildModifiersHaveMinMaxRestrictions { get; set; }

        /// <summary>
        /// Modifiers group code.
        /// </summary>
        [JsonProperty(PropertyName = "responseType", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Code { get; set; }
    }
}
