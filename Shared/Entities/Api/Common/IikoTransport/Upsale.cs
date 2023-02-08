using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WebAppAssembly.Shared.Entities.Api.Common.IikoTransport
{
    public class Upsale
    {
        /// <summary>
        /// Id of action that caused the suggestion
        /// </summary>
        [JsonProperty("sourceActionId")]
        [JsonPropertyName("sourceActionId")]
        public Guid SourceActionId { get; set; }
        /// <summary>
        /// Suggestion text
        /// </summary>
        [JsonProperty("suggestionText")]
        [JsonPropertyName("suggestionText")]
        public string? SuggestionText { get; set; }
        /// <summary>
        /// Codes of products that suggested to be added to order
        /// </summary>
        [JsonProperty("productCodes")]
        [JsonPropertyName("productCodes")]
        public IEnumerable<string>? ProductCodes { get; set; }
    }
}
