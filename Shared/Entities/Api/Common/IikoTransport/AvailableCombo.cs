using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WebAppAssembly.Shared.Entities.Api.Common.IikoTransport
{
    public class AvailableCombo
    {
        /// <summary>
        /// Id of combo specification, describing combo content
        /// </summary>
        [JsonProperty("specifiecationId")]
        [JsonPropertyName("specifiecationId")]
        public Guid SpecifiecationId { get; set; }
        /// <summary>
        /// Groups contained in combo. If null - there is no suitable product in order yet for that group
        /// </summary>
        [JsonProperty("groupMapping")]
        [JsonPropertyName("groupMapping")]
        public IEnumerable<GroupMapping>? GroupMapping { get; set; }
    }
}
