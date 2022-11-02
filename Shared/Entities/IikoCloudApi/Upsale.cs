namespace WebAppAssembly.Shared.Entities.IikoCloudApi
{
    public class Upsale
    {
        /// <summary>
        /// Id of action that caused the suggestion
        /// </summary>
        public Guid SourceActionId { get; set; }
        /// <summary>
        /// Suggestion text
        /// </summary>
        public string? SuggestionText { get; set; }
        /// <summary>
        /// Codes of products that suggested to be added to order
        /// </summary>
        public IEnumerable<string>? ProductCodes { get; set; }
    }
}
