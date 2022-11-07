using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WebAppAssembly.Shared.Entities.EMenu
{
    public class OutOfStock
    {
        [JsonProperty("correlationId")]
        [JsonPropertyName("correlationId")]
        public Guid CorrelationId { get; set; }
        [JsonProperty("terminalGroupStopLists")]
        [JsonPropertyName("terminalGroupStopLists")]
        public IEnumerable<TerminalGroupStopList>? TerminalGroupStopLists { get; set; }
    }
}
