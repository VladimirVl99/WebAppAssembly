using Newtonsoft.Json;

namespace WebAppAssembly.Shared.Entities.Api.Common.IikoTransport.Loyalties
{
    /// <summary>
    /// Suggested item to add or advices for customer.
    /// </summary>
    [JsonObject]
    public class Upsale
    {
        /// <summary>
        /// Id of action that caused the suggestion
        /// </summary>
        [JsonProperty(PropertyName = "sourceActionId", Required = Required.Always)]
        public Guid SourceActionId { get; set; }

        /// <summary>
        /// Suggestion text
        /// </summary>
        [JsonProperty(PropertyName = "suggestionText", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? SuggestionText { get; set; }

        /// <summary>
        /// Codes of products that suggested to be added to order
        /// </summary>
        [JsonProperty(PropertyName = "productCodes", Required = Required.Always)]
        public IEnumerable<string> ProductCodes { get; set; } = default!;
    }
}
