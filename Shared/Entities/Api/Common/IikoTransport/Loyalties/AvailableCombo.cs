using Newtonsoft.Json;

namespace WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.Loyalties
{
    /// <summary>
    /// Partially added combo, available for assembly.
    /// </summary>
    [JsonObject]
    public class AvailableCombo
    {
        /// <summary>
        /// Id of combo specification, describing combo content.
        /// </summary>
        [JsonProperty(PropertyName = "specifiecationId", Required = Required.Always)]
        public Guid SpecifiecationId { get; set; }

        /// <summary>
        /// Groups contained in combo.
        /// If null - there is no suitable product in order yet for that group.
        /// </summary>
        [JsonProperty(PropertyName = "groupMapping", Required = Required.Always)]
        public IEnumerable<GroupMapping> GroupMapping { get; set; } = default!;
    }
}
